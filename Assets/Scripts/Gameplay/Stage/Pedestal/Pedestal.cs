using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] private Transform playerHeldTransformExample;
    [SerializeField] private Transform playerBeforeJumpingTransformExample;
    [SerializeField] private float initPosA;
    [SerializeField] private float initPosB;
    private void Start()
    {
        float x = initPosA;
        if (Random.Range(0, 2) == 0) x = initPosB;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.OnEnterPedestalTrigger(playerBeforeJumpingTransformExample.position);
        }
    }
}
