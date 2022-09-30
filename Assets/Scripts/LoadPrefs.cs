using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class LoadPrefs : MonoBehaviour
{

    public static LoadPrefs instance;
    public GameObject menu;

    [SerializeField] private bool canUse = false;

    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTxtValue = null;
    public PostProcessProfile brightNess;
    public PostProcessLayer layer;
    AutoExposure exposure;

    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;


    void Awake(){

        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(menu);

        LoadData();
    }

    public void LoadData(){
        if(canUse){
            if(PlayerPrefs.HasKey("MasterVolume")){
                float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

                mixer.SetFloat(Menu.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);

            }
            
            if(PlayerPrefs.HasKey("MusicVolume")){
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

                mixer.SetFloat(Menu.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);

            }

            if(PlayerPrefs.HasKey("SFXVolume")){
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

                mixer.SetFloat(Menu.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);

            }

            if(PlayerPrefs.HasKey("masterQuality")){
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if(PlayerPrefs.HasKey("masterFullScreen")){
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullScreen == 1){
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else{
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if(PlayerPrefs.HasKey("masterBrightness")){
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                
                brightNess.TryGetSettings(out exposure);

                brightnessSlider.value = localBrightness;

                brightnessTxtValue.text = localBrightness.ToString("0.0");
            }
        }
    }
}
