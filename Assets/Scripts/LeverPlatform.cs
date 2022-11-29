using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverPlatform : MonoBehaviour
{
    public Transform bridge;
    public Animator animator;
    private PlayerInputAction playerInputs;

    private bool leverTrigger, doLever;

    void Awake(){
        playerInputs = new PlayerInputAction();
    }

    // Start is called before the first frame update
    void Start()
    {
        bridge = GameObject.Find("Bridge").transform;
    }

    void Update() {
        if(doLever){
            bridge.Rotate(new Vector3 (0, 0,- Time.deltaTime * 20));
        }
        if(bridge.eulerAngles.z < 1){
            doLever = false;
        }
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += BridgeInteraction;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    void BridgeInteraction(InputAction.CallbackContext context){
        if(leverTrigger){
            doLever = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            leverTrigger = true;
        } else{
            leverTrigger = false;
        }
    }
}
