using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float stepLength = 0.5f;
    [SerializeField] private float stepSpeed = 50f;

    private bool moving = false;

    private void Update()
    {
        TakeInput();
    }

    private void TakeInput()
    {
        if (moving) return;

        if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Move(Vector3.right));
        else if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Move(-Vector3.right));
        else if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Move(Vector3.forward));
        else if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Move(-Vector3.forward));
    }

    private IEnumerator Move(Vector3 direction)
    {
        yield return new WaitForFixedUpdate();

        moving = true;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + direction * stepLength;
        float distancePerFrame = stepSpeed * Time.fixedDeltaTime;

        float movedDistance = 0f;
        while (movedDistance < stepLength)
        {
            Vector3 position = transform.position;

            position += direction * distancePerFrame;
            movedDistance += distancePerFrame;

            if (movedDistance > stepLength) position = targetPosition;

            transform.position = position;

            yield return new WaitForFixedUpdate();
        }

        moving = false;
    }
}