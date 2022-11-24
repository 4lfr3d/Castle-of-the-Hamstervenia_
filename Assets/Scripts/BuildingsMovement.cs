using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingsMovement : MonoBehaviour
{
    public GameObject Enter;

    private PlayerInputAction playerInputs;
    private GameObject taroSale;
    private bool swotch;
    private Vector3 lugar;

    void Awake(){
        playerInputs = new PlayerInputAction();
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += teleport;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    void teleport(InputAction.CallbackContext context){
        if(swotch){
            lugar = Enter.transform.position;
            taroSale.transform.position = lugar;
            Debug.Log(lugar);
            swotch = false;
        }
    }

    private void OnTriggerStay2D(Collider2D TuGfa){
        if(TuGfa.gameObject.tag == "Player"){
            taroSale = TuGfa.gameObject;
            swotch = true;
        }
        else{
            swotch = false;
        }
    }
}
