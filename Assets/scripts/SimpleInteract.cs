using UnityEngine;

public class SimpleInteractable : MonoBehaviour
{
    [SerializeField] private TR tr;
    [SerializeField] private int riddleNumber;
    [SerializeField] private int answerValue;
    [SerializeField] private bool alwaysWrong;

    public void Interact()
    {
        if (tr == null) return;
        if (tr.IsRiddlePlaying) return;

        if (alwaysWrong)
        {
            tr.SubmitAnswer(-1);
            return;
        }

        if (tr.CurrentRiddle != riddleNumber)
        {
            tr.SubmitAnswer(-1);
            return;
        }

        tr.SubmitAnswer(answerValue);
    }
}