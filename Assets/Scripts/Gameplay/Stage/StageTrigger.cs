using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private GameplayManager.StageData stageData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameplayManager.Get().SetNewStage(stageData);
    }
}