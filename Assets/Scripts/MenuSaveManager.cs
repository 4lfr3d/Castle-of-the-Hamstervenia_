using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSaveManager : MonoBehaviour
{
    [Space(5)]
    public string NextScene;
    [Header("General Settings")]
    [SerializeField] private GameObject noSaveGame = null;
    
    //New Game
    public void StartGame(){
        GameObject.Find("StartMenu").gameObject.GetComponent<SaveManager>().DeleteSaveData();
        SceneManager.LoadScene(NextScene);
    }

    //Load Game
    public void LoadGame(){
        //view players loaded data (level/zone/checkpoint)

        //load player's scene
        if(GameObject.Find("StartMenu").gameObject.GetComponent<SaveManager>().LoadChecker()){
            SceneManager.LoadScene(NextScene);
        } else{
            noSaveGame.SetActive(true);
        }
    }

    //Quit Game
    public void QuitGame(){
        //Debug.Log("Afuera pana");
        Application.Quit();
    }
}
