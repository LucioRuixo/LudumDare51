using UnityEngine;

public class PressurePlateDoor : MonoBehaviour
{
    [SerializeField] private PressurePlate[] pressurePlates;
    [SerializeField] GameObject doorBlockingTrigger;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        int enabledPlate = Random.Range(0, pressurePlates.Length);

        for (int i = 0; i < pressurePlates.Length; i++)
        {
            pressurePlates[i].Door = this;
            if (i == enabledPlate) pressurePlates[i].Enable();
            else pressurePlates[i].Disable();
        }
    }

    public void Open()
    {
        animator.SetTrigger("OpenDoor");
        doorBlockingTrigger.SetActive(false);
    }

    public void Close()
    {

    }
}