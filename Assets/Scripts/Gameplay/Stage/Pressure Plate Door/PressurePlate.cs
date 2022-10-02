using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Collider trigger;

    private bool isEnabled;

    public PressurePlateDoor Door { get; set; }

    public void OnPlayerEnteredTrigger()
    {
        Debug.Log("PLAYER ENTERED TRIGGER");
        Door.Open();
    }

    public void Enable()
    {
        trigger.enabled = true;
    }

    public void Disable()
    {
        trigger.enabled = false;
    }
}