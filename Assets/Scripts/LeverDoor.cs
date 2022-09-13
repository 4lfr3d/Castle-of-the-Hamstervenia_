using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoor : MonoBehaviour
{
    private Transform door;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.Find("DoorLever");
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                Destroy(door.gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                Destroy(door.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if(door!=null){
            if(Input.GetKeyDown(KeyCode.V) && col.gameObject.tag == "Player")
            {
		    	animator.SetBool("lever", true); //Animator Lever check by Omar
                Destroy(door.gameObject);
            }
        }
    }
}
