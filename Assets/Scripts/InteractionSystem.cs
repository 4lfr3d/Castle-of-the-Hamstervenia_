using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{

    //Detection : Point , Radius, Layer
    [Header("Detection Params")]
    public Transform detectionPoint;
    public const float detectionRadius = 3f;
    public LayerMask detectionLayer;
     
    public GameObject detectedObject;
    private PlayerInputAction playerInputs;

    private void Awake(){
        playerInputs = new PlayerInputAction();
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += InteractInput;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    void InteractInput(InputAction.CallbackContext context){
        if(DetectObject()){
            detectedObject.GetComponent<Item>().Interact();
        }
    }

    bool DetectObject(){
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,
        detectionRadius, detectionLayer);

        if(obj == null){
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}
