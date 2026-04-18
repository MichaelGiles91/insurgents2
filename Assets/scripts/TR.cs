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

    bool isRiddlePlaying;
    bool isPlayer;
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
        }
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
