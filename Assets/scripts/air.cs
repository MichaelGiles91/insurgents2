using UnityEngine;

public class Air : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference is missing on Air.");
            return;
        }

        if (!tr.CanAnswer)
        {
            Debug.Log("Air too early (riddle not ready for answers)");
            return;
        }

        Debug.Log("Air selected (ANSWER 2)");

        tr.SubmitAnswer(2);
    }
}