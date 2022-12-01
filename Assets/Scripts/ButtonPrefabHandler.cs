using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrefabHandler : MonoBehaviour
{
    private InventorySystem taroInv;
    private Store taroStore;
    private Forge taroForge;
    private DialogueTrigger taroDialogue;
    private DialogueManager dialogueMan;
    private GameController gameController;
    private SaveManager savemang;

    private TaroHealth taroHealth;
    
    // Update is called once per frame
    void Update()
    {
        if(taroInv == null){
            taroInv = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        }
        if(taroStore == null){
            taroStore = GameObject.FindGameObjectWithTag("Player").GetComponent<Store>();
        }
        if(taroForge == null){
            taroForge = GameObject.FindGameObjectWithTag("Player").GetComponent<Forge>();
        }
        if(taroDialogue == null){
            taroDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueTrigger>();
        }
        if(taroHealth == null){
            taroHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<TaroHealth>();
        }
        if(savemang == null){
            savemang = GameObject.FindGameObjectWithTag("Player").GetComponent<SaveManager>();
        }
        if(dialogueMan == null){
            dialogueMan = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        }
    }

    //Inv System
    public void InvShowDescription(int id){
        taroInv.ShowDescription(id);
    }
    public void InvEquipedItem(int id){
        taroInv.EquipedItem(id);
    }
    public void InvHideDescription(int id){
        taroInv.HideDescription(id);
    }

    //DialogStuff
    public void ChosenOption(){
        taroDialogue.OpcionElegida();
    }

    public void showtienda(){
        taroDialogue.showtienda();
    }
    public void showDialogo(){
        taroDialogue.showDialogo();
    }

    public void NextSentence(){
        dialogueMan.DisplayNextSentence();
    }

    //Store
    public void ExitStore(){
        taroStore.ExitStore();
    }
    public void Trueque(int id){
        taroStore.Trueque(id);
    }

    //Forge
    public void ExitForge(){
        taroForge.ExitForge();
    }
    public void displayUpgrade(){
        taroForge.displayUpgrade();
    }
    public void Mejora(int id){
        taroForge.Mejora(id);
    }

    //MultiplayerController

    public void SalirMultiJugador(){
        GameObject.Find("GameController").GetComponent<GameController>().WinGame();
    }

    //save

    public void SaveChosenOption(){
        savemang.OpcionGuardadoElegida();
    }

    public void SaveProgress(){
        savemang.Save();
    }


    //Respawn
    public void Respawn(){
        taroHealth.Respawn();
    }
}
