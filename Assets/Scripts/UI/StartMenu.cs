using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string levelName;
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        LoadOptions();
        Time.timeScale = 1f;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            SaveOptions();
        }
        masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }
    public void UpdateMixerVolume()
    {
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;

        UpdateMixerVolume();
        SaveOptions();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;

        UpdateMixerVolume();
        SaveOptions();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;

        UpdateMixerVolume();
        SaveOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startGame()
    {
        SceneManager.LoadScene(levelName);
    }
    public void quitGame()
    {
        Application.Quit();

    }
}
