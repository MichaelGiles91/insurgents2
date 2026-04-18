using UnityEngine;

public class GunPickup : MonoBehaviour, IPickup
{
    public gunStats gun;

    public void pickup(PlayerController player)
    {
        ObjectiveManager.instance.CompleteObjective();
        player.getGunStats(gun);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.getGunStats(gun);
                Destroy(gameObject);
            }
        }
    }
}