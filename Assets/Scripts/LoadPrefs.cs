using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;

public class LoadPrefs : MonoBehaviour
{
    [SerializeField] private bool canUse = false;
    [SerializeField] private Menu menu;

    [SerializeField] private TMP_Text volumeTxtValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTxtValue = null;
    public PostProcessProfile brightNess;
    public PostProcessLayer layer;
    AutoExposure exposure;

    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;


    void Awake(){
        if(canUse){
            if(PlayerPrefs.HasKey("masterVolume")){
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTxtValue.text = localVolume.ToString("0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else {
                menu.ResetBtn("Audio");
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
