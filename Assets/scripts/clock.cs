using UnityEngine;

public class Clock : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;
    [SerializeField] private int answerValue;
    [SerializeField] private int correctAnswer;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference missing on Clock");
            return;
        }

        // Block during riddle playback
        if (tr.IsRiddlePlaying)
            return;

        // Only allow Riddle 1 logic here
        if (tr.CurrentRiddle != 1)
        {
            tr.SubmitAnswer(-1);
            return;
        }

        tr.SubmitAnswer(answerValue);
    }
}