using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float stepLength = 0.5f;
    [SerializeField] private float stepSpeed = 50f;

    private bool moving = false;
    private bool canHide = false;
    private bool isHidden = false;
    HidingSpot currentHidingSpot = null;
    private Vector3 posBeforeHiding;
    private Vector3 lastPosition;
    private float yPos = 1.0f; //posicion en Y en que se va a mantener siempre le player


    private void Start()
    {
        yPos = transform.position.y;
    }

    private void Update()
    {
        TakeInput();
    }

    private void TakeInput()
    {

        if (moving) return;

        if (Input.GetKeyDown(KeyCode.Space)) StopHiding();

        if (isHidden) return;

        if (Input.GetKeyDown(KeyCode.D)) MoveInDir(Vector3.right);
        else if (Input.GetKeyDown(KeyCode.A)) MoveInDir(-Vector3.right);
        else if (Input.GetKeyDown(KeyCode.W)) MoveInDir(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.S)) MoveInDir(-Vector3.forward);
        
    }

    private IEnumerator Move(Vector3 pos)
    {
        yield return new WaitForFixedUpdate();

        moving = true;
        lastPosition = transform.position;
        float distancePerFrame = stepSpeed * Time.fixedDeltaTime;
        Vector3 initialPos = transform.position;
        float t = 0.0f;
        while (t < 1.0)
        {
            Debug.Log(t);
            t += distancePerFrame;
            if (t > 1) t = 1;

            transform.position = Vector3.Lerp(initialPos, pos, t);

            yield return new WaitForFixedUpdate();
        }

        moving = false;
        if (canHide) Hide();
    }
    
    private void MoveInDir(Vector3 dir)
    {
        MoveToPos(transform.position + dir * stepLength);
    }

    private void MoveToPos(Vector3 pos)
    {
        StartCoroutine(Move(new Vector3(pos.x, yPos, pos.z)));
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
        Debug.Log("stop hiding");

        MoveToPos(posBeforeHiding);
        canHide = false;
        isHidden = false;
        currentHidingSpot = null;
    }
}