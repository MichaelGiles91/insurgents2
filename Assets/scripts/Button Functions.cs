using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject PauseMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("CentralHubScene");
    }
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    public void quit()
    {
        SceneManager.LoadScene("HomeMenuScene");
    }
    public void respawnPlayer()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();    
    }
    public void OpenSettings()
    {
        Debug.Log("OpenSettings fired");
        PauseMenu.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        
        settingsPanel.SetActive(false);
        PauseMenu.SetActive(true);
    }
}
