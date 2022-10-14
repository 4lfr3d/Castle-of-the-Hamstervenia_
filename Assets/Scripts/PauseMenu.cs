using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject inventoryUI;
    public GameObject canvasGameUI;
    private PlayerInputAction playerInputs;

    void Awake(){
        pauseMenuUI.SetActive(false);
        playerInputs = new PlayerInputAction();
    }

    private void OnEnable(){
        playerInputs.Player.Menu.performed += DoMenu;
        playerInputs.Player.Menu.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Menu.Disable();
    }

    public void DoMenu(InputAction.CallbackContext context){
        if(GameIsPaused){
            Resume();
        }
        else{
            Inventory();
        }
    }

    public void Resume(){
        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        canvasGameUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Settings(){
        settingsUI.SetActive(true);
    }

    public void Inventory(){
        Pause();
        settingsUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        canvasGameUI.SetActive(false);
        inventoryUI.SetActive(false);
        settingsUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame(){
        Application.Quit();
    }

    
}
