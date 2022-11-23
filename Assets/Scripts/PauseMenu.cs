using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    

    [Header("Game Panels")]
    public GameObject canvasGameUI;
    public GameObject pauseMenuUI;
    public bool GameIsPaused = false;


    [Header("Pause Menu Panels")]
    public GameObject settingsUI;
    public GameObject inventoryUI;
    public GameObject gameplayUI;
    public GameObject mapUI;

    public int pest = 0;    
    
    [Header("Pause Menu BG Img")]
    public Image settingsIconBG;
    public Image gameplayIconBG;
    public Image mapIconBg;
    public Image inventoryIconBG;

    private float fadeTime = 1f;

    [Header("Interaction Panels")]
    public GameObject storePanel;

        
    private PlayerInputAction playerInputs;

    void Awake(){
        pauseMenuUI = GameObject.Find("PauseMenu");
        settingsUI = GameObject.Find("Settings");
        inventoryUI = GameObject.Find("Inventario");
        canvasGameUI = GameObject.Find("UIPlayer");
        gameplayUI = GameObject.Find("Controls");
        mapUI = GameObject.Find("Map");
        storePanel = GameObject.Find("Store");

        settingsIconBG = GameObject.Find("Settings_Icon_BG").GetComponent<Image>();
        gameplayIconBG = GameObject.Find("Gameplay_Icon_BG").GetComponent<Image>();
        mapIconBg = GameObject.Find("Mapa_Icon_BG").GetComponent<Image>();
        inventoryIconBG = GameObject.Find("Inventario_Icon_BG").GetComponent<Image>();


        playerInputs = new PlayerInputAction();

        pauseMenuUI.SetActive(false);
        mapUI.SetActive(false);
        gameplayUI.SetActive(false);
        settingsUI.SetActive(false);
       
        DOTween.Init();

        settingsIconBG.DOFade(0f,0f);
        gameplayIconBG.DOFade(0f,0f);
        mapIconBg.DOFade(0f,0f);
        inventoryIconBG.DOFade(0f,0f);
        

    }

    private void OnEnable(){
        playerInputs.Player.Menu.performed += DoMenu;
        playerInputs.Player.Menu.Enable();
    }

    private void OnEnableUI(){
        playerInputs.Menú.Exit.performed += DoMenu;
        playerInputs.Menú.Exit.Enable();


        playerInputs.Menú.PastPest.performed += PrevPest;
        playerInputs.Menú.PastPest.Enable();

        playerInputs.Menú.NextPest.performed += NexPest;
        playerInputs.Menú.NextPest.Enable();

    }

    private void OnDisableUI(){
        playerInputs.Menú.Exit.Disable();
        playerInputs.Menú.NextPest.Disable();
        playerInputs.Menú.PastPest.Disable();
    }

    private void OnDisable(){
        playerInputs.Player.Menu.Disable();
    }

    public void DoMenu(InputAction.CallbackContext context){
        if(GameIsPaused){
            Resume();
            playerInputs.Menú.Disable();
            playerInputs.Player.Enable();
            OnDisableUI();
        }
        else{
            Inventory();
            playerInputs.Player.Disable();
            playerInputs.Menú.Enable();
            OnEnableUI();
        }
    }

    public void Resume(){
        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        if (!storePanel.active)
        {
            canvasGameUI.SetActive(true);
        }
        GameIsPaused = false;
    }

    public void Inventory(){
        Pause();
        inventoryUI.SetActive(true);
        inventoryIconBG.DOFade(0.5f,fadeTime);
    }

    public void Pause(){
        canvasGameUI.SetActive(false);
        pauseMenuUI.SetActive(true);        
        settingsUI.SetActive(false);
        GameIsPaused = true;
    }


    private void PrevPest(InputAction.CallbackContext context){

        if(pest > 0){
            pest--;
        }

        switch(pest){
            case 0:
                inventoryUI.SetActive(true);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                inventoryIconBG.DOFade(0.5f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);
                break;
            case 1:
                inventoryUI.SetActive(false);
                mapUI.SetActive(true);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0.5f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);

                break;

            case 2:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(true);
                settingsUI.SetActive(false);


                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0.5f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);

                break;
            case 3:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(true);


                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0.5f,fadeTime);
                break;
        }

    }

    private void NexPest(InputAction.CallbackContext context){

        if(pest < 3){
            pest++;
        }
        
        switch(pest){
            case 0:
                inventoryUI.SetActive(true);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                inventoryIconBG.DOFade(0.5f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);
                break;
            case 1:
                inventoryUI.SetActive(false);
                mapUI.SetActive(true);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0.5f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);

                break;

            case 2:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(true);
                settingsUI.SetActive(false);

                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0.5f,fadeTime);
                settingsIconBG.DOFade(0f,fadeTime);
                break;
            case 3:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(true);

                inventoryIconBG.DOFade(0f,fadeTime);
                mapIconBg.DOFade(0f,fadeTime);
                gameplayIconBG.DOFade(0f,fadeTime);
                settingsIconBG.DOFade(0.5f,fadeTime);
                break;
        }

    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame(){
        Application.Quit();
    }

    
}
