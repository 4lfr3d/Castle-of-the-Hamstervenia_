using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    LineRenderer line;
    private PlayerInputAction playerInputs;

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] GameObject flecha;

    bool isGrappling = false;
    bool enemySmall = false;
    [HideInInspector] public bool retracting = false;

    RaycastHit2D hit;
    Vector3 target;

    private void Awake(){
        line = GetComponent<LineRenderer>();
        playerInputs = new PlayerInputAction();
    }

    private void Update()
    {
        if (retracting)
        {
            if(enemySmall){
                target = hit.transform.position;
                Vector3 grapplePos = Vector3.Lerp(target, transform.position, grappleSpeed * Time.deltaTime);
                hit.transform.position = grapplePos;
            } else{
                Vector3 grapplePos = Vector3.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);
                transform.position = grapplePos;
            }

            line.SetPosition(0, transform.position);
            Debug.Log(Vector3.Distance(transform.position, target));

            if (enemySmall == false && Vector3.Distance(transform.position, target) < 15f)
            {
                enemySmall = false;
                retracting = false;
                isGrappling = false;
                line.enabled = false;
            }
            else if(enemySmall == true && Vector3.Distance(target, transform.position) < 50f){
                retracting = false;
                isGrappling = false;
                line.enabled = false;
            } 
        }
    }

    private void OnEnable(){
        playerInputs.Player.Gancho.performed += EnabledFlecha;
        playerInputs.Player.Gancho.canceled += StartGrapple;
        playerInputs.Player.Gancho.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Gancho.Disable();
    }

    private void EnabledFlecha(InputAction.CallbackContext context){
        flecha.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void StartGrapple(InputAction.CallbackContext context)
    {
        flecha.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if(!isGrappling){
            Vector3 tempValue = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempValue.z = 0f;
            Vector3 direction = tempValue - transform.position;

            hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableMask);

            if (hit.collider != null)
            {
                Menu.instance.GanchoSFX();
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemySmall")){
                    isGrappling = true;
                    target = hit.point;
                    line.enabled = true;
                    line.positionCount = 2;
                    enemySmall = true;

                    StartCoroutine(Attract());
                } else{
                    isGrappling = true;
                    target = hit.point;
                    line.enabled = true;
                    line.positionCount = 2;
                    enemySmall = false;

                    StartCoroutine(Grapple());
                }
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

    IEnumerator Attract(){
        float t = 0;
        float time = 10;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        for(; t < time; t += grappleShootSpeed * Time.deltaTime){
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.transform.position);
            yield return null;
        }

        line.SetPosition(1, transform.position);
        retracting = true;
    }
}
