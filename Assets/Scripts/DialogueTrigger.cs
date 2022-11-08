using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    
    //change to when player gets close to npc

    public List<Dialogue> dialogue;

    public Store store;

    public GameObject conversationIcon;

    private PlayerInputAction playerInputs;

    public GameObject dialogoptions;

    private Dialogue selector;

    private bool chat;

    private bool opcion;
    private bool opcion2;

    private string nombreVendedor;
    private string tagNpc;

    private void Awake(){
        playerInputs = new PlayerInputAction();
        chat = false;
        dialogoptions.SetActive(false);
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += TriggerDialogue;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    public void showtienda(){
        Debug.Log("Display store");
        if(nombreVendedor == "Talpa") store.displayStore();
    }

    public void showDialogo(){
        Debug.Log("Display dialogue");
        DialogoSelector();
        DialogueManager.instance.StartDialogue(selector);
    }

    public void DialogoSelector(){
        foreach (Dialogue dialogo in dialogue)
        {
            if(nombreVendedor == dialogo.name){
                selector = dialogo;
            }
        }
    }

    public void TriggerDialogue(InputAction.CallbackContext context){

        DialogoSelector();
        if(chat){
            if(tagNpc == "NPCVendor") dialogoptions.SetActive(true);
            else DialogueManager.instance.StartDialogue(selector);
        }        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "NPCVendor" || other.tag == "NPCs")
        {
            nombreVendedor = other.gameObject.name;
            tagNpc = other.tag;
            chat = true;
            conversationIcon.gameObject.transform.position = other.gameObject.transform.position + new Vector3(-20, 75, 0);
            conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } 
        else
        {
            chat = false;
           conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
