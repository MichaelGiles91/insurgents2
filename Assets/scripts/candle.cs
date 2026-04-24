using UnityEngine;

public class Candle : MonoBehaviour, IInteractable
{
  [SerializeField] private TR tr;

  public void Interact()
  {
    Debug.Log("Candle selected (ANSWER 3)");

    if (tr == null)
    {
      Debug.LogError("TR reference is missing on Candle.");
      return;
    }

    tr.SubmitAnswer(3);
  }
}