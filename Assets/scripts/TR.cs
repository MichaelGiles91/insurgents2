using UnityEngine;
using TMPro;

public class TR : MonoBehaviour
{
    [SerializeField] GameObject recorder;
    [SerializeField] AudioSource Tape;
    [SerializeField] AudioSource VO;
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
    float Timer;

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
        if (isPlayer && Input.GetKeyDown(KeyCode.E))
        {
            if (!Tape.isPlaying && !VO.isPlaying)
            {
                Tape.Play();
                VO.Play();
            }
        }

        if (isRiddlePlaying)
        {
            Timer += Time.deltaTime;
            if (Timer >= R1ST)
            {
                VO.Play();
            }

            if (Timer >= R1ET)
            {
                VO.Stop();
            }

            if (Timer >= R2ST)
            {
                VO.Play();
            }

            if (Timer >= R2ET)
            {
                VO.Stop();
            }

            if (Timer >= R3ST)
            {
                VO.Play();
            }

            if (Timer >= R3ET)
            {
                VO.Stop();
            }

            if (Timer >= EDST)
            {
                VO.Play();
            }

            if (Timer >= EDET)
            {
                VO.Stop();
            }

            if(clock == selected && RA1 == true)
            {
              VO.Play();
            }

            else
            {
              
            }

        }
    }

    void RiddleAnswer()
    {
      clock = RA1;
      air = RA2;
      candle = RA3;
    }

 

    private void OnTriggerEnter(Collider other)
    {
      if(other.CompareTag("Player"))
        { 
          isPlayer = true;
          recorder.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          isPlayer = false;
          recorder.SetActive(false);
        }
    }
}
