using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] private Transform PlayerHeldTransformExample;
    [SerializeField] private Transform PlayerBeforeJumpingTransformExample;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.OnEnterPedestalTrigger(PlayerBeforeJumpingTransformExample.position);
        }
    }
}
