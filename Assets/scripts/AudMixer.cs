using UnityEngine;
using UnityEngine.Audio;
public class AudMixer : MonoBehaviour
{
    public AudioMixer mixer;
    
    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float SFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMusicVolume(music);
        SetSFXVolume(SFX);
    }

    // Update is called once per frame
  public void SetMusicVolume(float value)
    {
        Debug.Log("Music slider moved: " + value);
        float volume = Mathf.Log10(value) * 20;
        mixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume" , value);

    }
    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        mixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);

    }
}
