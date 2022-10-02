using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [Space]
    [SerializeField] private Vector3 offset;
    private float initialX;

    private void Start()
    {
        initialX = transform.position.x;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position = new Vector3(initialX, pivot.position.y + offset.y, pivot.position.z + offset.z);
        transform.position = position;
    }
}