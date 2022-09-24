using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{

    [Header("General Settings")]
    [SerializeField] private GameObject noSaveGame = null;


    //Volume Settings
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTxtValue = null;
    
    [SerializeField] private Slider volumeSlider = null;    
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private AudioMixer introMusic;


    //Gameplay Settings
    [Header("Language Settings")]
    private bool active = false;


    //Graphics Settings
    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private int _qualityLevel;

    [Header("FullScreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;
    private bool _isFullScreen;


    //Brightness Slider
    [Header("Brightness Settings")]
    [SerializeField] private TMP_Text brightnessTxtValue = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defaultBright = 1;
    private float _brightnessLevel;
    public PostProcessProfile brightNess;
    public PostProcessLayer layer;
    AutoExposure exposure;

    //Resolution Drpdn
    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;


    //Confirmation Box for applying changes
    [Header("Confirmation Box")]
    [SerializeField] private GameObject confirmationPrompt = null;

    void Awake(){
        volumeSlider.onValueChanged.AddListener(SetMusicIntro);
    }

    //Start
    private void Start(){

        //Language
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);

        //Brightness
        brightNess.TryGetSettings(out exposure);
        AdjustBrightness(brightnessSlider.value);

        //Resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height){
                currResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        //Audio
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1.0f);

    }   


    //New Game
    public void StartGame(){
        SceneManager.LoadScene("firstScene");
    }


    //Load Game
    public void LoadGame(){
        //view players loaded data (level/zone/checkpoint)

        //load player's scene

        //no loaded data
        noSaveGame.SetActive(true);
    }


    //Quit Game
    public void QuitGame(){
        //Debug.Log("Afuera pana");
        Application.Quit();
    }


    //Volume
    public void SetMusicIntro(float volume){
        introMusic.SetFloat("MusicIntro", Mathf.Log10(volume) * 20);
    }

    public void VolumeApply(){
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        StartCoroutine(Confirmation());
    }

    //Reset Btn
    public void ResetBtn(string MenuType){
        if(MenuType == "Audio"){
            introMusic.SetFloat("MusicIntro", Mathf.Log10(defaultVolume) * 20);            
            volumeSlider.value = defaultVolume;
            //volumeTxtValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(MenuType == "Graphics"){
            brightnessSlider.value = defaultBright;
            brightnessTxtValue.text = defaultBright.ToString("0.0");
            exposure.keyValue.value = .5f;

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
    }

    //Gameplay
    public void ChangeLocale(int localeID){
        if (active)return;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID){
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        PlayerPrefs.SetInt("LocaleKey", _localeID);
        active = false;
    }

    public void GameplayApply(){
        StartCoroutine(Confirmation());
    }

    //Graphics
    public void AdjustBrightness(float value){
        if(value != 0 ){
            exposure.keyValue.value = value;
        }
        else{
            exposure.keyValue.value = .5f;
        }

        _brightnessLevel = exposure.keyValue.value;
        brightnessTxtValue.text = value.ToString("0.0");

    }

/*
    public void SetBrightness(float brightness){
        _brightnessLevel = brightness;
        brightnessTxtValue.text = brightness.ToString("0.0");
    }
*/

    public void SetFullScreen(bool isFullScreen){
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void GraphicsApply(){
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //later change w/ post processing

        PlayerPrefs.SetInt("masterQuality",_qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", _isFullScreen ? 1 : 0);
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(Confirmation());
    }

    //Confirmation Prompt
    public IEnumerator Confirmation(){
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

}