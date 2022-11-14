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
    public GameObject interactionButton;

    private bool detectObject = false;

    private PlayerInputAction playerInputs;

    private void Awake(){
        interactionButton = GameObject.Find("manoAgarrar");

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
        if(detectObject){
            detectedObject.GetComponent<Item>().Interact();
        }
    }

    void Update(){
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,
        detectionRadius, detectionLayer);
        if (obj != null)
        {
            //Debug.Log(obj.name);
            if(obj.name == "Croquetas"){
                Debug.Log(obj.name);
                this.gameObject.GetComponent<InventorySystem>().PickUp(obj.gameObject);
                obj.gameObject.SetActive(false);
            }
            detectedObject = obj.gameObject;
            detectObject = true;
            interactionButton.gameObject.transform.position = obj.gameObject.transform.position + new Vector3(0, 25, 0);
            interactionButton.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            detectedObject = null;
            detectObject = false;
            interactionButton.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}
