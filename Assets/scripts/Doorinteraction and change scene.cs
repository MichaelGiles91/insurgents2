using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Doorinteractionandchangescene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string sceneToLoad;

    public GameObject interactText;

    private bool playerInRange = false;

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}
