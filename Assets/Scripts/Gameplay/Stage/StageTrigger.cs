using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private GameplayManager.StageData stageData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBody playerBody)) GameplayManager.Get().SetNewStage(stageData);
    }
}