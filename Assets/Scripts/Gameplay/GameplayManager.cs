using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    [Header("Timer")]
    [SerializeField] private TimerHandle timer;

    private void Awake()
    {
        timer.OnTimerEnd += OnTimerEnd;

    }

    private void OnTimerEnd()
    {
    }

    private void ResetTimer()
    {
        timer.StartTimer();
    }
}