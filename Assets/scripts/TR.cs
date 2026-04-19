using UnityEngine;
using TMPro;

public class TR : MonoBehaviour
{
    [SerializeField] GameObject recorder;
    [SerializeField] GameObject Clock;
    [SerializeField] AudioSource Tape;
    [SerializeField] GameObject Sofa;
    [SerializeField] GameObject Air;
    [SerializeField] GameObject Candle;
    [SerializeField] GameObject CurrI;
    [SerializeField] int CR;
    [SerializeField] float RTL = 30f;
    [SerializeField] float R1ST;
    [SerializeField] float R2ST;
    [SerializeField] float R3ST;
    [SerializeField] float R1ET;
    [SerializeField] float R2ET;
    [SerializeField] float R3ET;
    [SerializeField] float EDST;
    [SerializeField] float EDET;
    [SerializeField] float RI1ST;
    [SerializeField] float RI2ST;
    [SerializeField] float RI3ST;
    [SerializeField] float RI1ET;
    [SerializeField] float RI2ET;
    [SerializeField] float RI3ET;
    [SerializeField] float RC1st;
    [SerializeField] float RC2ST;
    [SerializeField] float RC3ST;
    [SerializeField] float RC1ET;
    [SerializeField] float RC2ET;
    [SerializeField] float RC3ET;

    bool isRiddlePlaying;
    bool isPlayer;
    bool RA1;
    bool RA2;
    bool RA3;
    bool clock;
    bool air;
    bool candle;
    bool selected;
    bool riddleActive;

    float Timer;
    float riddleTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      if(Tape == null)
        {
          Tape = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    if (isRiddlePlaying)
      {
        Timer += Time.deltaTime;
        HandleRiddleAudio();
      }
        
      if (riddleActive)
      {
        riddleTimer += Time.deltaTime;

        if (riddleTimer >= RTL)
        {
          FailRiddle();
        }

      }

      Clock.SetActive(!Tape.isPlaying);
      Sofa.SetActive(!Tape.isPlaying);
      Air.SetActive(!Tape.isPlaying);
      Candle.SetActive(!Tape.isPlaying);

        if (isPlayer && Input.GetKeyDown(KeyCode.E))
        {
            if (!Tape.isPlaying)
            {
                Tape.Play();
            }
        }

        if (isRiddlePlaying)
        {
            Timer += Time.deltaTime;
            if (Timer >= R1ST)
            {
                Tape.Play();
            }
        }
    }

    void HandleRiddleAudio()
    {
        if (CR == 1)
        {
            if (Timer >= R1ST && !Tape.isPlaying)
                Tape.Play();

            if (Timer >= R1ET)
                Tape.Stop();
        }
        else if (CR == 2)
        {
            if (Timer >= R2ST && !Tape.isPlaying)
                Tape.Play();

            if (Timer >= R2ET)
                Tape.Stop();
        }
        else if (CR == 3)
        {
            if (Timer >= R3ST && !Tape.isPlaying)
                Tape.Play();

            if (Timer >= R3ET)
                Tape.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        isPlayer = true;
        recorder.SetActive(true);
      }

      if (other.gameObject == Sofa ||
         other.gameObject == Air ||
         other.gameObject == Candle ||
         other.gameObject == Clock)
      {
        CurrI = other.gameObject;
      }
    }
    
    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        isPlayer = false;
        recorder.SetActive(false);
      }

      if (other.gameObject == CurrI)
      {
        CurrI = null;
      }
    }

    void StartRiddle()
    {
      riddleActive = true;
      riddleTimer = 0f;
    }

    void FailRiddle()
    {
      riddleActive = false;

      Tape.Stop();

      Tape.time = RI1ST;
      Tape.Play();

      Debug.Log("FAILED RIDDLE - PLAYER DEAD");
    }

    public void RiddleAnswer(bool correct)
    {
      if (correct)
      {
        CompleteRiddle();
      }

      else
      {
        FailRiddle();
      }
    }

    void CompleteRiddle()
    {
      riddleActive = false;

      Tape.Stop();

      Tape.time = RC1st;
      Tape.Play();

      Debug.Log("RIDDLE COMPLETED");

      CR++;
      riddleTimer = 0f;
    }

    public void SelectClock()
    {
      RiddleAnswer(RA1);
    }

    public void SelectAir()
    {
      RiddleAnswer(RA2);
    }

    public void SelectCandle()
    {
      RiddleAnswer(RA3);
    }
}
