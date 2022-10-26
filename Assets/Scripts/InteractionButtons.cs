using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButtons : MonoBehaviour
{
    //Detection : Point , Radius, Layer
    [Header("Detection Params")]
    public Transform detectionPoint;
    public const float detectionRadius = 3f;
    public LayerMask detectionLayer;
    public GameObject interactionButton;

    void Awake()
    {
        interactionButton.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update(){
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,
        detectionRadius, detectionLayer);
        if(obj != null){
            interactionButton.gameObject.transform.position = obj.gameObject.transform.position + new Vector3(0, 25, 0);
            interactionButton.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else{
            interactionButton.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}
