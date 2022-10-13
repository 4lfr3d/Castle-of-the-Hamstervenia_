using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    LineRenderer line;
    private PlayerInputAction playerInputs;

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 3f;
    [SerializeField] float grappleShootSpeed = 5f;

    bool isGrappling = false;
    [HideInInspector] public bool retracting = false;

    Vector3 target;

    private void Awake(){
        line = GetComponent<LineRenderer>();
        playerInputs = new PlayerInputAction();
    }

    private void Update()
    {
        if (retracting)
        {
            Vector3 grapplePos = Vector3.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);

            transform.position = grapplePos;

            line.SetPosition(0, transform.position);

            if (Vector3.Distance(transform.position, target) < 15f)
            {
                retracting = false;
                isGrappling = false;
                line.enabled = false;
            }
        }
    }

    private void OnEnable(){
        playerInputs.Player.Gancho.performed += StartGrapple;
        playerInputs.Player.Gancho.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Gancho.Disable();
    }

    private void StartGrapple(InputAction.CallbackContext context)
    {
        if(!isGrappling){
            Vector3 tempValue = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempValue.z = 0f;
            Vector3 direction = tempValue - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableMask);

            if (hit.collider != null)
            {
                isGrappling = true;
                target = hit.point;
                line.enabled = true;
                line.positionCount = 2;

                StartCoroutine(Grapple());
            }
        } 
    }

    IEnumerator Grapple(){
        float t = 0;
        float time = 10;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector3 newPos;

        for(; t < time; t += grappleShootSpeed * Time.deltaTime){
            newPos = Vector3.Lerp(transform.position, target, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, target);
        retracting = true;
    }
}
