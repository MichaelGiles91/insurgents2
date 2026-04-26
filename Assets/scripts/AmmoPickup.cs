using UnityEngine;

public class AmmoPickup : MonoBehaviour, IPickup
{
    [SerializeField] gunStats gunType;
    [SerializeField] int ammoAmount = 10;
    [SerializeField] GameObject pickupText;
    [SerializeField] GameObject closedCrate;
    [SerializeField] GameObject openCrate;

    public void pickup(PlayerController player)
    {
        player.addAmmo(gunType, ammoAmount);

        if (pickupText != null)
            pickupText.SetActive(false);

        closedCrate.SetActive(false);
        openCrate.SetActive(true);

        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupText == null) return;
            pickupText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupText == null) return;
            pickupText.SetActive(false);
        }
    }
}