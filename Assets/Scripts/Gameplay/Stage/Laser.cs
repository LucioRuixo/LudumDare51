using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector3 movementDir;
    float levelEnd = -15.0f;

    private void FixedUpdate()
    {
        transform.position += movementDir * movementSpeed * Time.fixedDeltaTime;
        if (transform.position.z < levelEnd) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnHittedByHazard();
        }
    }
}
