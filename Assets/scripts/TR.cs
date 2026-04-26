using UnityEngine;

public class TR : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource Tape;

    [SerializeField] float R1ST, R1ET;
    [SerializeField] float R2ST, R2ET;
    [SerializeField] float R3ST, R3ET;

    [SerializeField] private float RTL = 300f;

    private float riddleTimer;
    private float currentStart;
    private float currentEnd;

    private bool isRiddleActive = false;
    private bool hasAnswered = false;
    private bool hasEvaluated = false;

    public bool CanAnswer { get; private set; }

    private int selectedAnswer = -1;
    private int correctAnswer = -1;

    private int currentRiddle = 1;

    private void Start()
    {
        if (Tape == null)
            Tape = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isRiddleActive) return;

        riddleTimer += Time.deltaTime;

        if (riddleTimer >= RTL)
        {
            Debug.Log("TIMER EXPIRED");
            FailRiddle();
            return;
        }

        if (!hasEvaluated && Tape.time >= currentEnd)
        {
            hasEvaluated = true;
            CanAnswer = true;
            Tape.Stop();
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
        hasAnswered = false;
        hasEvaluated = false;

        CanAnswer = false;

        selectedAnswer = -1;
        riddleTimer = 0f;

        switch (currentRiddle)
        {
            case 1:
                currentStart = R1ST;
                currentEnd = R1ET;
                correctAnswer = 1;
                break;

            case 2:
                currentStart = R2ST;
                currentEnd = R2ET;
                correctAnswer = 3;
                break;

            case 3:
                currentStart = R3ST;
                currentEnd = R3ET;
                correctAnswer = 4;
                break;
        }

        Tape.time = currentStart;
        Tape.Play();

        Debug.Log("Riddle " + currentRiddle + " Started");
    }

    public void SubmitAnswer(int answer)
    {
        Debug.Log("SubmitAnswer: " + answer);

        if (!CanAnswer) return;

        selectedAnswer = answer;
        hasAnswered = true;
    }

    private void EvaluateAnswer()
    {
        if (!isRiddleActive) return;

        isRiddleActive = false;
        CanAnswer = false;   // 🔥 LOCK AGAIN AFTER EVALUATION

        if (Tape != null)
            Tape.Stop();

        Debug.Log("Selected: " + selectedAnswer + " | Correct: " + correctAnswer);

        if (!hasAnswered)
        {
            FailRiddle();
            return;
        }

        if (selectedAnswer == correctAnswer)
        {
            CompleteRiddle();
        }
        else
        {
            FailRiddle();
        }
    }

    private void CompleteRiddle()
    {
        Debug.Log("RIDDLE COMPLETED");

        CanAnswer = false;

        currentRiddle++;

        if (currentRiddle > 3)
        {
            Debug.Log("ALL RIDDLES COMPLETE");
            return;
        }

        StartRiddle();
    }

    private void FailRiddle()
    {
        isRiddleActive = false;
        CanAnswer = false;

        if (Tape != null)
            Tape.Stop();

        Debug.Log("RIDDLE FAILED (DEATH)");
    }
}