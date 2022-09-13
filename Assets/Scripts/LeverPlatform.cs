using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPlatform : MonoBehaviour
{
    private Transform door;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.Find("Platforms");
        door.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                door.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                door.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                door.gameObject.SetActive(true);
            }
        }
    }
}
