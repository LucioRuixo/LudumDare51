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

    public enum StageTypes
    {
        Regular,
        Final
    }

    [Serializable]
    public struct StageData
    {
        public StageTypes type;

        public FrontalBaldie frontalBaldie;
        public UpperBaldie upperBaldie;
    }

    [Header("Timer")]
    [SerializeField] private TimerHandle timer;

    [Header("Gameplay Settings")]
    [SerializeField] private float unsafePhaseDuration = 3f;

    private bool safe = true;

    private BaldieTypes currentUnsafePhaseType = BaldieTypes.Frontal;
    private BaldieTypes lastUnsafePhaseType = BaldieTypes.Frontal;

    [SerializeField] private StageData initialStage;
    [SerializeField] private StageData currentStage;

    public bool Safe => safe;
    public BaldieTypes CurrentUnsafePhaseType => currentUnsafePhaseType;

    public static event Action<BaldieTypes> OnUnsafePhaseStart;

    public override void Awake()
    {
        base.Awake();

        timer.OnTimerEnd += StartUnsafePhase;
    }

    private void Start()
    {
        currentStage = initialStage;
    }

    private void StartUnsafePhase()
    {
        switch (currentStage.type)
        {
            case StageTypes.Regular:
                currentUnsafePhaseType = BaldieTypes.Frontal;
                currentStage.frontalBaldie.ToggleFacingState();
                break;
            case StageTypes.Final:
                if (lastUnsafePhaseType == BaldieTypes.Frontal)
                {
                    currentUnsafePhaseType = BaldieTypes.Upper;
                    currentStage.upperBaldie.ToggleFacingState();
                }
                else
                {
                    currentUnsafePhaseType = BaldieTypes.Frontal;
                    currentStage.frontalBaldie.ToggleFacingState();
                }
                break;
            default: break;
        }

        safe = false;

        StartCoroutine(UnsafePhase(currentUnsafePhaseType));
    }

    private void OnUnsafePhaseEnd()
    {
        switch (currentStage.type)
        {
            case StageTypes.Regular:
                currentStage.frontalBaldie.ToggleFacingState();
                break;
            case StageTypes.Final:
                if (lastUnsafePhaseType == BaldieTypes.Frontal) currentStage.upperBaldie.ToggleFacingState();
                else currentStage.frontalBaldie.ToggleFacingState();
                break;
            default: break;
        }

        lastUnsafePhaseType = lastUnsafePhaseType == BaldieTypes.Frontal ? BaldieTypes.Upper : BaldieTypes.Frontal;
        safe = true;

        ResetTimer();
    }

    private void ResetTimer()
    {
        timer.StartTimer();
    }

    public void SetNewStage(StageData newStage)
    {
        currentStage = newStage;
    }

    #region Coroutines
    private IEnumerator UnsafePhase(BaldieTypes type)
    {
        OnUnsafePhaseStart?.Invoke(type);

        yield return new WaitForSeconds(unsafePhaseDuration);

        OnUnsafePhaseEnd();
    }
    #endregion
}