using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [Space]
    [Tooltip("El valor de esta variable solo se evalúa al principio del juego")]
    [SerializeField] private bool freezeX;
    [Tooltip("El valor de esta variable solo se evalúa al principio del juego")]
    [SerializeField] private bool freezeZ;
    [SerializeField] private Vector3 offset;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float x = freezeX ? initialPosition.x : pivot.position.x + offset.x;
        float z = freezeZ ? initialPosition.z : pivot.position.z + offset.z;

        Vector3 position = transform.position;
        position = new Vector3(x, initialPosition.y, z);
        transform.position = position;
    }
}