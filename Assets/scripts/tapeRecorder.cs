using UnityEngine;
using TMPro;

public class tapeRecorder : MonoBehaviour
{
    [SerializeField] GameObject recorder;
    [SerializeField] AudioSource Tape;
    bool isPlayer;

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
      if(isPlayer && Input.GetKeyDown(KeyCode.E))
        {
          if(!Tape.isPlaying)
            {
             Tape.Play();
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
