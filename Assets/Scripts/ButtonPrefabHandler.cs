using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrefabHandler : MonoBehaviour
{
    private InventorySystem taroInv;
    private Store taroStore;
    private Forge taroForge;
    private DialogueTrigger taroDialogue;
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
    }

    //Inv System
    public void InvShowDescription(int id){
        taroInv.ShowDescription(id);
    }
    public void InvEquipedItem(int id){
        taroInv.EquipedItem(id);
    }
    public void InvHideDescription(){
        taroInv.HideDescription();
    }

    //DialogStuff
    public void showtienda(){
        taroDialogue.showtienda();
    }
    public void showDialogo(){
        taroDialogue.showDialogo();
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
    public void Mejora(int id){
        taroForge.Mejora(id);
    }
}
