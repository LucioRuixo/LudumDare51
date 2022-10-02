using UnityEngine;

public class PressurePlateDoor : MonoBehaviour
{
    [SerializeField] private PressurePlate[] pressurePlates;

    private void Start()
    {
        int enabledPlate = Random.Range(0, pressurePlates.Length - 1);
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            pressurePlates[i].Door = this;

            if (i == enabledPlate) pressurePlates[i].Enable();
            else pressurePlates[i].Disable();
        }
    }

    public void Open()
    {
        
    }

    public void Close()
    {

    }
}