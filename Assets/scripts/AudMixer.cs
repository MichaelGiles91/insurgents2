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



        musicSlider.value = music;
        SFXSlider.value = SFX;

        SetMusicVolume(music);
        SetSFXVolume(SFX);

    }

    // Update is called once per frame
    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
