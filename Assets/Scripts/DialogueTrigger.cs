using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    
    //change to when player gets close to npc

    public Dialogue dialogue;

    public GameObject conversationIcon;

    private PlayerInputAction playerInputs;

    private void Awake(){
        playerInputs = new PlayerInputAction();
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += TriggerDialogue;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }


    public void TriggerDialogue(InputAction.CallbackContext context){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "NPCs")
        {
           conversationIcon.gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 75, 0);
           conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        } else
        {
           conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
