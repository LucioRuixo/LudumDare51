using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public enum BaldieTypes
    {
        Frontal,
        Upper
    }

    [Header("Timer")]
    [SerializeField] private TimerHandle timer;

    [Header("Enemies")]
    [SerializeField] private UpperBaldie[] upperBaldies;
    [SerializeField] private FrontalBaldie[] frontalBaldies;

    [Header("Gameplay Settings")]
    [SerializeField] private float unsafePhaseDuration = 3f;

    private bool safe = true;
    [SerializeField] private BaldieTypes currentUnsafePhaseType = BaldieTypes.Frontal;

    public bool Safe => safe;
    public BaldieTypes CurrentUnsafePhaseType => currentUnsafePhaseType;

    public static event Action<BaldieTypes> OnUnsafePhaseStart;

    public override void Awake()
    {
        base.Awake();

        timer.OnTimerEnd += StartUnsafePhase;
    }

    private void StartUnsafePhase()
    {
        switch (currentUnsafePhaseType)
        {
            case BaldieTypes.Frontal:
                foreach (FrontalBaldie frontalBaldie in frontalBaldies) frontalBaldie.ToggleFacingState();
                break;
            case BaldieTypes.Upper:
                foreach (UpperBaldie upperBaldie in upperBaldies) upperBaldie.ToggleFacingState();
                break;
            default: break;
        }

        safe = false;

        StartCoroutine(UnsafePhase(currentUnsafePhaseType));
    }

    private void OnUnsafePhaseEnd()
    {
        switch (currentUnsafePhaseType)
        {
            case BaldieTypes.Frontal:
                foreach (FrontalBaldie frontalBaldie in frontalBaldies) frontalBaldie.ToggleFacingState();
                break;
            case BaldieTypes.Upper:
                foreach (UpperBaldie upperBaldie in upperBaldies) upperBaldie.ToggleFacingState();
                break;
            default: break;
        }

        safe = true;

        ResetTimer();
    }

    private void ResetTimer()
    {
        timer.StartTimer();
    }

    private IEnumerator UnsafePhase(BaldieTypes type)
    {
        OnUnsafePhaseStart?.Invoke(type);

        yield return new WaitForSeconds(unsafePhaseDuration);

        OnUnsafePhaseEnd();
    }
}