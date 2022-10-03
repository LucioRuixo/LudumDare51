using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiWalkTrigger : MonoBehaviour
{
    [SerializeField] private bool CanWalkRight = true;
    [SerializeField] private bool CanWalkLeft = true;
    [SerializeField] private bool CanWalkUp = true;
    [SerializeField] private bool CanWalkDown = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.RestrictMovement(CanWalkRight, CanWalkLeft, CanWalkUp, CanWalkDown);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.UnRestrictMovement(CanWalkRight, CanWalkLeft, CanWalkUp, CanWalkDown);
        }
    }
}
