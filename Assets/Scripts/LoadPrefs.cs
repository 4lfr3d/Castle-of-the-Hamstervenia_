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

    [SerializeField] public bool canUse = false;

    [SerializeField] public AudioMixer mixer;

    [SerializeField] public Slider brightnessSlider = null;
    [SerializeField] public TMP_Text brightnessTxtValue = null;
    public PostProcessProfile brightNess;
    public PostProcessLayer layer;
    AutoExposure exposure;

    [SerializeField] public TMP_Dropdown qualityDropdown;
    [SerializeField] public Toggle fullScreenToggle;


    void Awake(){

        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(menu);
            DontDestroyOnLoad(brightnessSlider);
        }
        else{
            Destroy(gameObject);
        }


        LoadData();
    }

    public void LoadData(){
        if(canUse){
            if(PlayerPrefs.HasKey("MasterVolume")){
                float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

               // Menu.SetMasterVolume(masterVolume);

                mixer.SetFloat(Menu.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);

            }
            
            if(PlayerPrefs.HasKey("MusicVolume")){
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

                //Menu.SetMusicVolume(musicVolume);
                mixer.SetFloat(Menu.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);

            }

            if(PlayerPrefs.HasKey("SFXVolume")){
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

               // Menu.SetSFXVolume(sfxVolume);
                
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

                brightnessTxtValue.text = localBrightness + "/100";
            }
        }
    }
}
