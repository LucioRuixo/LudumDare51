using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private GameplayManager.BaldieTypes hidingType;
    [SerializeField] private Animator animator;
    public GameplayManager.BaldieTypes HidingType => hidingType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBody>().Controller.OnEnterHidingSpotTrigger(this);
            //aca hariamos la animacion de la cortina abriendose y cerrandose, asumo
        }
    }

    public void Animate()
    {
        if (animator) animator.SetTrigger("Open");
    }

}