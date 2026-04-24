using UnityEngine;

public class TR : MonoBehaviour, IInteractable
{
  [SerializeField] private AudioSource Tape;

  [SerializeField] private int correctAnswer = 1;
  [SerializeField] private float RTL = 30f;

  private float riddleTimer;
  private float tapeTimer;

  private bool isRiddleActive;
  private bool isTapePlaying;

  private void Start()
  {
    if (Tape == null)
    Tape = GetComponent<AudioSource>();
  }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.E) && !isRiddleActive)
      {
        StartRiddle();
      }

      if (isRiddleActive)
      {
        riddleTimer += Time.deltaTime;

        if (riddleTimer >= RTL)
        {
          FailRiddle();
        }
      }

      if (isTapePlaying)
      {
        tapeTimer += Time.deltaTime;
      }
    }

    public void Interact()
    {
      if (!isRiddleActive)
      {
        StartRiddle();
      }
    }

    public void StartRiddle()
  {
    isRiddleActive = true;
    isTapePlaying = true;

    riddleTimer = 0f;
    tapeTimer = 0f;

    Debug.Log("Riddle Started");

    if (Tape != null)
    Tape.Play();
  }

  public void SubmitAnswer(int answer)
  {
    if (!isRiddleActive)
    return;

    if (answer == correctAnswer)
    {
      CompleteRiddle();
    }
        
    else
    {
      FailRiddle();
    }
  }

  public void FailRiddle()
  {
    if (!isRiddleActive)
    return;

    isRiddleActive = false;
    isTapePlaying = false;

    Debug.Log("RIDDLE FAILED (DEATH)");

    if (Tape != null)
    Tape.Stop();
  }

  private void CompleteRiddle()
  {
    isRiddleActive = false;
    isTapePlaying = false;

    Debug.Log("RIDDLE COMPLETED");

    if (Tape != null)
    Tape.Stop();
  }
}