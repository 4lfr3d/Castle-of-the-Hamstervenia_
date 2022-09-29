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
    [Header("Audio Settings")]
    [SerializeField] public float defaultVolume = 1.0f;
    [SerializeField] public AudioMixer mixer;

    [SerializeField] public TMP_Text masterTxtValue = null;
    [SerializeField] public TMP_Text musicTxtValue = null;
    [SerializeField] public TMP_Text sfxTxtValue = null;
    
    [SerializeField] public Slider masterSlidder = null; 
    [SerializeField] public Slider musicSlidder = null;
    [SerializeField] public Slider sfxSlidder = null;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
   

    //Gameplay Settings
    [Header("Language Settings")]
    private bool active = false;


    //Graphics Settings
    [Header("Quality Settings")]
    [SerializeField] public TMP_Dropdown qualityDropdown;
    public int _qualityLevel;

    [Header("FullScreen Settings")]
    [SerializeField] public Toggle fullScreenToggle;
    public bool _isFullScreen;


    //Brightness Slider
    [Header("Brightness Settings")]
    public float _brightnessLevel;
    public PostProcessProfile brightNess;
    public PostProcessLayer layer;
    AutoExposure exposure;

    [SerializeField] public TMP_Text brightnessTxtValue = null;
    [SerializeField] public Slider brightnessSlider = null;
    [SerializeField] public float defaultBright = 1.0f;

    //Resolution Drpdn
    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Resolution[] resolutions;

    //Confirmation Box for applying changes
    [Header("Confirmation Box")]
    [SerializeField] public GameObject confirmationPrompt = null;

    void Awake(){
        masterSlidder.onValueChanged.AddListener(SetMasterVolume);
        musicSlidder.onValueChanged.AddListener(SetMusicVolume);
        sfxSlidder.onValueChanged.AddListener(SetSFXVolume);
    }

    //Start
    public void Start(){

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
        masterSlidder.value = PlayerPrefs.GetFloat(MIXER_MASTER, 1.0f);
        musicSlidder.value = PlayerPrefs.GetFloat(MIXER_MUSIC, 1.0f);
        sfxSlidder.value = PlayerPrefs.GetFloat(MIXER_SFX, 1.0f);

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
    public void SetMasterVolume(float volume){
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);
        int sliderMasterValue = (int)(masterSlidder.value * 100);
        masterTxtValue.text =  sliderMasterValue + "/100"; 
    }

    public void SetMusicVolume(float volume){
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
        int sliderMusicValue = (int)(musicSlidder.value * 100);
        musicTxtValue.text =  sliderMusicValue + "/100";
    }

    public void SetSFXVolume(float volume){
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
        int sliderSfxValue = (int)(sfxSlidder.value * 100);
        sfxTxtValue.text =  sliderSfxValue + "/100";
    }

    public void VolumeApply(){
        PlayerPrefs.SetFloat("MasterVolume", masterSlidder.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlidder.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlidder.value);

        StartCoroutine(Confirmation());
    }

    //Reset Btn
    public void ResetBtn(string MenuType){
        if(MenuType == "Audio"){
            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(defaultVolume) * 20);
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(defaultVolume) * 20); 
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(defaultVolume) * 20);  

            masterSlidder.value = defaultVolume;
            musicSlidder.value = defaultVolume;
            sfxSlidder.value = defaultVolume;

            masterTxtValue.text = defaultVolume + "/100";
            musicTxtValue.text =  defaultVolume + "/100";
            sfxTxtValue.text =  defaultVolume + "/100";
            
            VolumeApply();
        }

        if(MenuType == "Graphics"){
            brightnessSlider.value = defaultBright;
            brightnessTxtValue.text = defaultBright.ToString("0.0");
            exposure.keyValue.value = defaultBright;

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
        int brightvalue = (int)(value * 20);
        brightnessTxtValue.text = brightvalue + "/100";

    }

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