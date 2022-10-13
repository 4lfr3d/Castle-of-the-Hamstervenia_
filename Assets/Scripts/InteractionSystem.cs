using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{

    //Detection : Point , Radius, Layer
    [Header("Detection Params")]
    public Transform detectionPoint;
    public const float detectionRadius = 2f;
    public LayerMask detectionLayer;
     
    public GameObject detectedObject;




    void Update()
    {
        if(DetectObject()){
            if(InteractInput()){
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool InteractInput(){
        // Para testing
        return Input.GetKeyDown(KeyCode.E);
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
