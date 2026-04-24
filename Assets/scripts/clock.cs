using UnityEngine;

public class Clock : MonoBehaviour, IInteractable
{
  [SerializeField] private TR tr;

  public void Interact()
  {
    Debug.Log("Clock selected (ANSWER 1)");

     if (tr == null)
     {
       Debug.LogError("TR reference is missing on Clock.");
       return;
     }

     tr.SubmitAnswer(1);
  }
}