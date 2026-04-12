using UnityEngine;

public class objectiveTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Triggered by: " + other.name);
            objectiveSystem.instance.nextObjective();
            Destroy(gameObject);
        }
    }
}
