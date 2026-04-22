using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject PauseMenu;
    public AudioSource aud;
    public AudioClip clickSFX;

    public void StartGame()
    {
        aud.PlayOneShot(clickSFX);
        StartCoroutine(LoadSceneSound("CentralHubScene"));
    }
    public void resume()
    {
        aud.PlayOneShot(clickSFX);
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        aud.PlayOneShot(clickSFX);
        gameManager.instance.stateUnpause();
        StartCoroutine(LoadSceneSound(SceneManager.GetActiveScene().name));
        
    }

    public void quit()
    {
        aud.PlayOneShot(clickSFX);
        StartCoroutine(LoadSceneSound("HomeMenuScene"));
    }
    public void respawnPlayer()
    {
        aud.PlayOneShot(clickSFX);
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();    
    }
    public void OpenSettings()
    {
        aud.PlayOneShot(clickSFX);
        PauseMenu.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        aud.PlayOneShot(clickSFX);
        settingsPanel.SetActive(false);
        PauseMenu.SetActive(true);
    }
    public void QuitGame()
    {
        StartCoroutine(QuitWithSound());

    }
    IEnumerator LoadSceneSound(string scene)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene(scene);
    }
    IEnumerator QuitWithSound()
    {
        aud.PlayOneShot(clickSFX);

        yield return new WaitForSecondsRealtime(0.2f);
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else 
    Application.Quit();
#endif
    }
}
