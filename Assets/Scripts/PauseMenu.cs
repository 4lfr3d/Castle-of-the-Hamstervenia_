using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    
    public bool GameIsPaused = false;

    public GameObject[] viewports; 
    
    public int pest = 0;

    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject inventoryUI;
    public GameObject canvasGameUI;
    public GameObject gameplayUI;
    public GameObject mapUI;

    private PlayerInputAction playerInputs;

    void Awake(){
        pauseMenuUI.SetActive(false);

        playerInputs = new PlayerInputAction();
        playerInputs.Menú.Disable();
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
        canvasGameUI.SetActive(true);
        GameIsPaused = false;
    }

    public void Settings(){
        settingsUI.SetActive(true);
    }

    public void Inventory(){
        Pause();
        inventoryUI.SetActive(true);
    }

    void Pause(){
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
                break;
            case 1:
                inventoryUI.SetActive(false);
                mapUI.SetActive(true);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                break;

            case 2:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(true);
                settingsUI.SetActive(false);
                break;
            case 3:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(true);
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
                break;
            case 1:
                inventoryUI.SetActive(false);
                mapUI.SetActive(true);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(false);

                break;

            case 2:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(true);
                settingsUI.SetActive(false);
                break;
            case 3:
                inventoryUI.SetActive(false);
                mapUI.SetActive(false);
                gameplayUI.SetActive(false);
                settingsUI.SetActive(true);
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
