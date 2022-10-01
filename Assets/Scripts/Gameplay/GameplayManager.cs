using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private enum MovementModes
    {
        Free,
        Loaded
    }

    [Header("Timer")]
    [SerializeField] private TimerHandle timer;

    [Header("Player")]
    [SerializeField] private PlayerController_FreeMovement playerController_FreeMovement;
    [SerializeField] private PlayerController_LoadedMovement playerController_LoadedMovement;
    [Space]
    [SerializeField] private MovementModes playerMovementMode;

    private void Awake()
    {
        timer.OnTimerEnd += OnTimerEnd;

        if (playerMovementMode == MovementModes.Loaded) playerController_LoadedMovement.OnMovementChainExecuted += ResetTimer;
    }

    private void Start()
    {
        if (playerMovementMode == MovementModes.Free)
        {
            playerController_FreeMovement.enabled = true;
            playerController_LoadedMovement.enabled = false;
        }
        else
        {
            playerController_FreeMovement.enabled = false;
            playerController_LoadedMovement.enabled = true;
        }
    }

    private void OnTimerEnd()
    {
        if (playerMovementMode == MovementModes.Loaded) playerController_LoadedMovement.StartExecutingMovementChain();
    }

    private void ResetTimer()
    {
        timer.StartTimer();
    }
}