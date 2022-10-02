using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    [SerializeField] private PressurePlate pressurePlate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) pressurePlate.OnPlayerEnteredTrigger();
    }
}