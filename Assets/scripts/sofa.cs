using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private TR tr;

    public void Interact()
    {
        if (tr == null)
        {
            Debug.LogError("TR reference missing on Sofa");
            return;
        }

        // Block during riddle playback
        if (tr.IsRiddlePlaying)
            return;

        // Always wrong answer no matter the riddle
        tr.SubmitAnswer(-1);
    }
}