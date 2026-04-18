using UnityEngine;

public class healthPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 25;
    [SerializeField] float respawnTime = 10f;

    MeshRenderer mesh;
    Collider col;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.healDamage(healAmount);
            }

            StartCoroutine(Respawn());
        }
    }

    System.Collections.IEnumerator Respawn()
    {
        // hide object
        mesh.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // bring it back
        mesh.enabled = true;
        col.enabled = true;
    }
}