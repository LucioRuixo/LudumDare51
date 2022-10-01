using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    public PlayerController Controller => controller;

    public void OnJumpEnd()
    {
        controller.OnJumpEnd();
    }
}