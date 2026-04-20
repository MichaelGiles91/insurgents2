using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectiveManager.instance.CompleteObjective();

            //if (GetComponent<GunPickup>() != null)
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}