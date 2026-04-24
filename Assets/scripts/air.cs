using UnityEngine;

public class Air : MonoBehaviour, IInteractable
{
  [SerializeField] private TR tr;

  public void Interact()
  {
    Debug.Log("Air selected (ANSWER 2)");

    if (tr == null)
    {
      Debug.LogError("TR reference is missing on Air.");
      return;
    }

    tr.SubmitAnswer(2);
  }
}