using UnityEngine;

public class Air : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;
    [SerializeField] private int answerValue;
    [SerializeField] private int correctAnswer;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference missing on Air");
            return;
        }

        // Block during riddle playback
        if (tr.IsRiddlePlaying)
            return;

        // Only allow Riddle 2 logic here
        if (tr.CurrentRiddle != 2)
        {
            tr.SubmitAnswer(-1);
            return;
        }

        tr.SubmitAnswer(answerValue);
    }
}