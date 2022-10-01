using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    private Animator animator;
    #endregion

    [SerializeField] private float stepLength = 0.5f;
    [SerializeField] private float stepSpeed = 50f;

    private bool isAlive = true;
    private bool moving = false;
    private bool canHide = false;
    private bool isHidden = false;

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
        GameplayManager.OnUnsafePhaseStart += CheckSafety;
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
        GameplayManager.OnUnsafePhaseStart -= CheckSafety;
    }

    private void TakeInput()
    {
        if (isHidden)
        {
            if (Input.GetKeyDown(KeyCode.Space)) StopHiding();
        }
        else if (!moving)
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
        MoveToPos(transform.position + dir * stepLength);
    }

    private void MoveToPos(Vector3 pos, Action OnEnd = null)
    {
        StartCoroutine(Move(new Vector3(pos.x, yPos, pos.z), OnEnd));
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
        if (!moving) Hide();
        currentHidingSpot = hidingSpot;
    }

    private void Hide()
    {
        posBeforeHiding = lastPosition;
        isHidden = true;
        canHide = false;
        MoveToPos(currentHidingSpot.transform.position);
    }

    private void StopHiding()
    {
        MoveToPos(posBeforeHiding, CheckSafety);

        canHide = false;
        isHidden = false;
        currentHidingSpot = null;
    }

    private void CheckSafety()
    {
        if (isHidden) return;

        if (!GameplayManager.Get().Safe) Die();
    }

    public void OnJumpEnd()
    {
        moving = false;
    }

    public void OnHittedByHazard()
    {
        if (!isHidden) Die(); //tiene potencial de romperse si le player sale del escondite mientras esta triggereando. Seguiria en el trigger pero ya no estaria Hidden entonces no moriria, polish 
    }

    private void Die()
    {
        isAlive = false;
        //aca llamaria la funcion de Game Over, si tuviera una
        Debug.Log("GAME OVER, PLAYER DIED");
    }

    #region Coroutines
    private IEnumerator Move(Vector3 pos, Action OnEnd = null)
    {
        yield return new WaitForFixedUpdate();

        moving = true;
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

        moving = false;
        if (canHide) Hide();

        OnEnd?.Invoke();
    }
    #endregion
}