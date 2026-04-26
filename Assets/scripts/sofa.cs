using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference missing on Sofa.");
            return;
        }

        if (!tr.CanAnswer)
        {
            Debug.Log("Sofa too early (not in answer phase)");
            return;
        }

        Debug.Log("SOFA TRAP ACTIVATED (WRONG ANSWER)");

        tr.SubmitAnswer(-999);
    }
}