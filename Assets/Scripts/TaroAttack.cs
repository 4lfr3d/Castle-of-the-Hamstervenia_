using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaroAttack : MonoBehaviour
{
    public Animator animator;

    private bool isTaroAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isTaroAttacking);
        //Weapon transform Taro
        this.transform.position=transform.parent.position;

        if(Input.GetKeyDown(KeyCode.V))
        {
			//transform.position = transform.position + new Vector3(0, 150, 0);
			animator.SetBool("isAttacking", true); //Animator Attack check by Omar
            attackingFun();
        }

        if(!AnimatorIsPlaying()){
            animator.SetBool("isAttacking", false); //Animator Attack check by Omar
            isTaroAttacking = false; 
        }

    }

    //Damage and hitbox stuff
    void attackingFun()
    {
        isTaroAttacking = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Destructible") && isTaroAttacking){
            Destroy(col.gameObject);
            isTaroAttacking = false; 
        }
    }
    private void OnTriggerStay2D(Collider2D col) {
        if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Destructible") && isTaroAttacking){
            Destroy(col.gameObject);
            isTaroAttacking = false; 
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Destructible") && isTaroAttacking){
            Destroy(col.gameObject);
            isTaroAttacking = false; 
        }
    }

    //Checks is animation ended
    bool AnimatorIsPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
