using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_LoadedMovement : MonoBehaviour
{
    private enum StepDirections
    {
        Forward,
        Back,
        Left,
        Right
    }

    private enum MovementStates
    {
        Loading,
        Moving
    }

    [SerializeField] private float stepLength = 0.5f;
    [SerializeField] private float stepSpeed = 50f;
    [SerializeField] private float stepInterval = 0.25f;

    private bool stepping;
    private MovementStates movementState = MovementStates.Loading;
    private List<StepDirections> movementChain = new List<StepDirections>();

    public event Action OnMovementChainExecuted;

    private void Update()
    {
        if (movementState == MovementStates.Loading) TakeInput();
    }

    private void TakeInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) movementChain.Add(StepDirections.Forward);
        else if (Input.GetKeyDown(KeyCode.S)) movementChain.Add(StepDirections.Back);
        else if (Input.GetKeyDown(KeyCode.A)) movementChain.Add(StepDirections.Left);
        else if (Input.GetKeyDown(KeyCode.D)) movementChain.Add(StepDirections.Right);
    }

    public void StartExecutingMovementChain()
    {
        movementState = MovementStates.Moving;

        StartCoroutine(ExecuteMovementChain());
    }

    #region Coroutines
    private IEnumerator ExecuteMovementChain()
    {
        while (movementChain.Count > 0)
        {
            stepping = true;
            StartCoroutine(Step(movementChain[0]));

            yield return new WaitUntil(() => !stepping);

            if (movementChain.Count > 1) yield return new WaitForSeconds(stepInterval);

            movementChain.RemoveAt(0);
        }

        movementState = MovementStates.Loading;

        OnMovementChainExecuted?.Invoke();
    }

    private IEnumerator Step(StepDirections stepDirection)
    {
        Vector3 direction = Vector3.zero;

        switch (stepDirection)
        {
            case StepDirections.Forward:
                direction = Vector3.forward;
                break;
            case StepDirections.Back:
                direction = -Vector3.forward;
                break;
            case StepDirections.Left:
                direction = -Vector3.right;
                break;
            case StepDirections.Right:
                direction = Vector3.right;
                break;
            default: break;
        }

        yield return new WaitForFixedUpdate();

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + direction * stepLength;
        float distancePerFrame = stepSpeed * Time.fixedDeltaTime;

        float movedDistance = 0f;
        while (movedDistance < stepLength)
        {
            Vector3 position = transform.position;

            position += direction * distancePerFrame;
            movedDistance += distancePerFrame;

            if (movedDistance > stepLength) position = initialPosition + direction * stepLength;

            transform.position = position;

            yield return new WaitForFixedUpdate();
        }

        stepping = false;
    }
    #endregion
}