using UnityEngine;

public class Candle : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference is missing on Candle.");
            return;
        }

        if (!tr.CanAnswer)
        {
            Debug.Log("Candle too early (riddle not ready for answers)");
            return;
        }

        Debug.Log("Candle selected (ANSWER 3)");

        tr.SubmitAnswer(3);
    }
}