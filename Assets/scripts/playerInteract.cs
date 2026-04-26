using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
  [SerializeField] private Camera cam;
  [SerializeField] private float range = 3f;

  void Start()
  {
    if (cam == null)
    cam = Camera.main;
  }

    void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      Ray ray = cam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out RaycastHit hit, range))
      { 
        Debug.Log("Ray hit: " + hit.collider.name);

        IInteractable interactable = hit.collider.GetComponent<IInteractable>();

        if (interactable != null)
        {
          Debug.Log("INTERACTING");
          interactable.Interact();
        }
                
        else
        {
          Debug.Log("NOT INTERACTABLE");
        }
      }
    }
  }
}