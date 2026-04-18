using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudMixer : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider SFXSlider;
    
    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float SFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMusicVolume(music);
        SetSFXVolume(SFX);

      //  musicSlider.value = music;
        SFXSlider.value = SFX;
    }

    // Update is called once per frame
  public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        float volume = Mathf.Log10(value) * 20;
        mixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume" , value);

    }
    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        float volume = Mathf.Log10(value) * 20;
        mixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);

    }
}
