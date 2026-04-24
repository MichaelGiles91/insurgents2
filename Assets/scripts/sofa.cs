using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
  [SerializeField] private TR tr;

  public void Interact()
  {
    Debug.Log("Sofa selected (NOT AN ANSWER)");

    if (tr == null)
    {
      Debug.LogError("TR reference is missing on Sofa.");
      return;
    }

    tr.FailRiddle();
  }
}