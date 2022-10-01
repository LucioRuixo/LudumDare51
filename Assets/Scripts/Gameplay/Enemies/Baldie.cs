using System.Collections;
using UnityEngine;

public class Baldie : MonoBehaviour
{
    private enum FacingStates
    {
        TurnedBack,
        TurnedForward
    }

    [SerializeField] private float turnSpeed = 10f;

    private bool turning = false;

    private FacingStates facingState = FacingStates.TurnedBack;

    private void StartTurningBack()
    {
        StartCoroutine(Turn(180f));
    }

    private void StartTurningForward()
    {
        StartCoroutine(Turn(-180f));
    }

    public void ToggleFacingState()
    {
        if (turning) return;

        if (facingState == FacingStates.TurnedBack) StartTurningForward();
        else StartTurningBack();
    }

    #region Coroutines
    private IEnumerator Turn(float angle)
    {
        yield return new WaitForFixedUpdate();

        turning = true;
        float anglesPerTick = turnSpeed * Time.fixedDeltaTime;
        float initialAngle = transform.rotation.eulerAngles.y;
        float targetAngle = initialAngle + angle;

        float rotatedAngle = 0f;
        while (rotatedAngle < Mathf.Abs(angle))
        {
            Vector3 rotation = transform.rotation.eulerAngles;

            rotatedAngle += Mathf.Abs(anglesPerTick);

            if (rotatedAngle <= Mathf.Abs(angle)) rotation.y += anglesPerTick;
            else rotation.y = targetAngle;

            transform.rotation = Quaternion.Euler(rotation);

            yield return new WaitForFixedUpdate();
        }

        turning = false;
    }
    #endregion
}