using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.OnEnterHidingSpotTrigger(this);
            //aca hariamos la animacion de la cortina abriendose y cerrandose, asumo
        }
    }

}