using UnityEngine;

public class Clock : MonoBehaviour, IInteractable
{
  [SerializeField] private TR tr;

  public void Interact()
  {
    if (tr == null)
    {
      Debug.LogError("TR reference is missing on Clock.");
      return;
    }

    if (!tr.CanAnswer)
    {
      Debug.Log("Clock too early (riddle not ready for answers)");
      return;
    }

    Debug.Log("Clock selected (ANSWER 1)");

    tr.SubmitAnswer(1);
    }
}