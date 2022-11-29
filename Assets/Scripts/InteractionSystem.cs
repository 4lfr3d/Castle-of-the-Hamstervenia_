using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class InteractionSystem : MonoBehaviour
{

    //Detection : Point , Radius, Layer
    [Header("Detection Params")]
    public Transform detectionPoint;
    public const float detectionRadius = 3f;
    public LayerMask detectionLayer;
     
    public GameObject detectedObject;
    public GameObject interactionButton;
    public SpriteRenderer interaction_Sprite;

    private bool detectObject = false;
    private float fadeTime = 0.5f;

    private PlayerInputAction playerInputs;

    private void Awake(){
        interactionButton = GameObject.Find("manoAgarrar");
        interaction_Sprite = GameObject.Find("manoAgarrar").GetComponent<SpriteRenderer>();

        playerInputs = new PlayerInputAction();

        DOTween.Init();

        interaction_Sprite.DOFade(0f,0f);
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
            interactionButton.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 60, 0);
            interaction_Sprite.DOFade(1.5f, fadeTime);
        }
        else
        {
            detectedObject = null;
            detectObject = false;
            interaction_Sprite.DOFade(0f,fadeTime);
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}
