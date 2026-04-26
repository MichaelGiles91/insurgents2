using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask ignoreLayer;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance, ~ignoreLayer))
        {
            Debug.Log("Interact hit: " + hit.collider.name);

            SimpleInteractable interactable = hit.collider.GetComponentInParent<SimpleInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}