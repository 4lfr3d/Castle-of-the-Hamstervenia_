using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSaveManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject noSaveGame = null;
    
    //New Game
    public void StartGame(){
        GameObject.Find("StartMenu").gameObject.GetComponent<SaveManager>().DeleteSaveData();
        SceneManager.LoadScene("firstScene");
    }


    //Load Game
    public void LoadGame(){
        //view players loaded data (level/zone/checkpoint)

        //load player's scene
        if(GameObject.Find("StartMenu").gameObject.GetComponent<SaveManager>().LoadChecker()){
            Debug.Log("GAME LOADED");
            SceneManager.LoadScene("firstScene");
        } else{
            noSaveGame.SetActive(true);
            Debug.Log("GAME NOT LOADED");
        }
    }


    //Quit Game
    public void QuitGame(){
        //Debug.Log("Afuera pana");
        Application.Quit();
    }
}