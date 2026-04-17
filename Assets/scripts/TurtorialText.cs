using UnityEngine;

public class TurtorialText : MonoBehaviour
{

    public GameObject TurtorialPanel;

  
    bool hasShown;
    void Update()
    {
        if (TurtorialPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            
            TurtorialPanel.SetActive(false);
        }

        
    }
    public void TutorialTextOpen()
    {
        TurtorialPanel.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasShown)
        {
            
            hasShown = true;
            TutorialTextOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            TurtorialPanel.SetActive(false);    
        }
    }
}
