using UnityEngine;

public class GunPickup : MonoBehaviour, IPickup
{
    public gunStats gun;
    [SerializeField] GameObject pickupText;

    bool isPlayerin;
    public void pickup(PlayerController player)
    {
        player.getGunStats(gun);

      
        pickupText.SetActive(false);

        Destroy(transform.root.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (pickupText == null)
            {
                return;
            }
            isPlayerin = true;
            pickupText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerin = false;
            pickupText.SetActive(false);
        }
    }
}