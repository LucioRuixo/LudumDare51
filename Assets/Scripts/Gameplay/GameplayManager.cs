using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    [Header("Timer")]
    [SerializeField] private TimerHandle timer;

    [Header("Enemies")]
    [SerializeField] private Baldie[] baldies;

    [Header("Gameplay Settings")]
    [SerializeField] private float unsafePhaseDuration = 3f;

    bool safe = true;
    public bool Safe => safe;

    public static event Action OnUnsafePhaseStart;

    public override void Awake()
    {
        base.Awake();

        timer.OnTimerEnd += StartUnsafePhase;
    }

    private void StartUnsafePhase()
    {
        foreach (Baldie baldie in baldies) baldie.ToggleFacingState();
        safe = false;

        StartCoroutine(UnsafePhase());
    }

    private void OnUnsafePhaseEnd()
    {
        foreach (Baldie baldie in baldies) baldie.ToggleFacingState();
        safe = true;

        ResetTimer();
    }

    private void ResetTimer()
    {
        timer.StartTimer();
    }

    private IEnumerator UnsafePhase()
    {
        OnUnsafePhaseStart?.Invoke();

        yield return new WaitForSeconds(unsafePhaseDuration);

        OnUnsafePhaseEnd();
    }
}