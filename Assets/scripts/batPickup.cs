using UnityEngine;

public class batPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();
        }

        if (player != null)
        {
            Debug.Log("Player found: " + player.name);
            player.getBat();
            Destroy(gameObject);
        }
    }
}