using UnityEngine;

public class GunPickup : MonoBehaviour, IPickup
{
    public gunStats gun;

    public void pickup(PlayerController player)
    {
        player.getGunStats(gun);

        ObjectiveManager.instance.CompleteObjective();

        Destroy(gameObject);
    }
}