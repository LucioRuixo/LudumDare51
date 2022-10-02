using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    private Animator animator;
    #endregion

    [SerializeField] private float stepLength = 0.5f;
    [SerializeField] private float stepSpeed = 50f;

    private bool isAlive = true;
    bool isAnimating = false;
    private bool moving = false;
    private bool canHide = false;
    private bool isHiddenFromFront = false;
    private bool isHiddenFromAbove = false;

    private HidingSpot currentHidingSpot = null;
    private Vector3 posBeforeHiding;
    private Vector3 lastPosition;

    private float yPos = 1.0f; //posicion en Y en que se va a mantener siempre el player

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        GameplayManager.OnUnsafePhaseStart += (GameplayManager.BaldieTypes type) => CheckSafety(type == GameplayManager.BaldieTypes.Frontal);
    }

    private void Start()
    {
        yPos = transform.position.y;
    }

    private void Update()
    {
        TakeInput();
    }

    private void OnDisable()
    {
        GameplayManager.OnUnsafePhaseStart -= (GameplayManager.BaldieTypes type) => CheckSafety(type == GameplayManager.BaldieTypes.Frontal);
    }

    private void TakeInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Get().TogglePauseState();
        }
        if (isHiddenFromFront || isHiddenFromAbove)
        {
            if (Input.GetKeyDown(KeyCode.E)) StopHiding();
        }
        else if (!moving && !isAnimating)
        {
            if (Input.GetKeyDown(KeyCode.W)) MoveInDir(Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.S)) MoveInDir(-Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.A)) MoveInDir(-Vector3.right);
            else if (Input.GetKeyDown(KeyCode.D)) MoveInDir(Vector3.right);
            else if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }
    }

    private void MoveInDir(Vector3 dir)
    {
        MoveToPos(transform.position + dir.normalized * stepLength);
    }

    private void MoveToPos(Vector3 pos, Action OnEnd = null, float delay = 0f)
    {
        StartCoroutine(Move(new Vector3(pos.x, yPos, pos.z), OnEnd, delay));
    }     

    private void Jump()
    {
        moving = true;
        animator.SetTrigger("Jump");
    }

    public void OnEnterHidingSpotTrigger(HidingSpot hidingSpot)
    {
        Debug.Log("enter hiding");
        canHide = true;
        if (!moving) Hide(hidingSpot.HidingType == GameplayManager.BaldieTypes.Frontal);
        currentHidingSpot = hidingSpot;
    }

    private void Hide(bool fromFront)
    {
        animator.SetTrigger("Hide");

        if (fromFront) isHiddenFromFront = true;
        else isHiddenFromAbove = true;

        currentHidingSpot.Animate();
        posBeforeHiding = lastPosition;
        canHide = false;
        MoveToPos(currentHidingSpot.transform.position, null, 0.25f);
    }

    private void StopHiding()
    {
        animator.SetTrigger("Stop Hiding");

        MoveToPos(posBeforeHiding, () => CheckSafety(GameplayManager.Get().CurrentUnsafePhaseType == GameplayManager.BaldieTypes.Frontal), 0.25f);

        canHide = false;
        isHiddenFromFront = false;
        isHiddenFromAbove = false;
        currentHidingSpot = null;
    }

    private void CheckSafety(bool fromFront)
    {
        if (fromFront && isHiddenFromFront) return;
        if (!fromFront && isHiddenFromAbove) return;

        if (!GameplayManager.Get().Safe) Die();
    }

    public void OnJumpEnd()
    {
        moving = false;
    }

    public void OnHittedByHazard()
    {
        if (!isHiddenFromFront && !isHiddenFromAbove) Die(); //tiene potencial de romperse si le player sale del escondite mientras esta triggereando. Seguiria en el trigger pero ya no estaria Hidden entonces no moriria, polish 
    }

    private void Die()
    {
        isAlive = false;
        //GameData.Get().SetWinState(false);  //cuando la escena este lista descomentar esto para tener lose condition
        Debug.Log("GAME OVER, PLAYER DIED");
    }

    public void OnEnterPedestalTrigger(Vector3 posToJumpTo)
    {
        StartCoroutine(JumpToPedestal(posToJumpTo));
    }

    public void OnWin()
    {
        GameData.Get().SetWinState(true);
        Debug.Log("YOU WIN");
    }

    #region Coroutines
    private IEnumerator Move(Vector3 pos, Action OnEnd = null, float delay = 0f)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        yield return new WaitForFixedUpdate();

        moving = true;
        animator.SetTrigger("Move");

        lastPosition = transform.position;
        float distancePerFrame = stepSpeed * Time.fixedDeltaTime;
        Vector3 initialPos = transform.position;

        float t = 0.0f;
        while (t < 1.0)
        {
            t += distancePerFrame;
            if (t > 1) t = 1;

            transform.position = Vector3.Lerp(initialPos, pos, t);

            yield return new WaitForFixedUpdate();
        }

        if (canHide)
        {
            yield return new WaitForSeconds(0.25f);
            Hide(currentHidingSpot.HidingType == GameplayManager.BaldieTypes.Frontal);
        }

        moving = false;

        OnEnd?.Invoke();
    }

    private IEnumerator JumpToPedestal(Vector3 posToJumpTo)
    {
        isAnimating = true;
        posToJumpTo = new Vector3(posToJumpTo.x, yPos, posToJumpTo.z);
        yield return new WaitUntil(() => moving == false);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForFixedUpdate();
        Vector3 distToPedestal = posToJumpTo - transform.position;
        
        int aux = 0;
        if (distToPedestal.z < stepLength - 0.05f)
        {
            aux = -1;
        }
        else if (distToPedestal.z > stepLength - 0.05f)
        {
            aux = 1;
        }
        while ((posToJumpTo - transform.position).z * aux > stepLength - 0.05f)
        {
            MoveInDir(new Vector3(0.0f, 0.0f, distToPedestal.normalized.z));
            yield return new WaitUntil(() => moving == false);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForFixedUpdate();
        }
        aux = 0;
        if (distToPedestal.x < stepLength - 0.05f)
        {
            aux = -1;
        }
        else if (distToPedestal.x > stepLength - 0.05f)
        {
            aux = 1;
        }
        while ((posToJumpTo - transform.position).x * aux > stepLength - 0.05f)
        {
            MoveInDir(new Vector3(distToPedestal.normalized.x, 0.0f, 0.0f));
            yield return new WaitUntil(() => moving == false);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForFixedUpdate();
        }
        MoveToPos(posToJumpTo);
        animator.SetTrigger("JumpToPedestal");
    }
    
    #endregion
}