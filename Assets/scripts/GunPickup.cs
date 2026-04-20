using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public gunStats gun;

    bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            player.getGunStats(gun);

            ObjectiveManager.instance.CompleteObjective();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}