using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject canvasGameUI;

    void Awake(){
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused){
                Resume();
            } else{
                Pause();
            }
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

    void Pause(){
        pauseMenuUI.SetActive(true);
        canvasGameUI.SetActive(false);
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
