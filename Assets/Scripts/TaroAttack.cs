using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class TaroAttack : MonoBehaviour
{
    // Cuca 3 Rata 5
    public Animator animator;
    private PlayerInputAction playerInputs;
    public InventorySystem inv;
    public int damage = 1;

    private bool isTaroAttacking = false;
    //private CoinsManager cm;

    private void Awake(){
        playerInputs = new PlayerInputAction();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isTaroAttacking);
        //Weapon transform Taro
        this.transform.position = transform.parent.position;

        if(!AnimatorIsPlaying()){
            animator.SetBool("isAttacking", false); //Animator Attack check by Omar
            isTaroAttacking = false; 
        }
        /*if(cm == null){
            cm = GameObject.Find("Coins").GetComponent<CoinsManager>();
        }*/

    }

    private void OnEnable(){
        playerInputs.Player.Attack.performed += DoAttack;
        playerInputs.Player.Attack.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Attack.Disable();
    }

    private void DoAttack(InputAction.CallbackContext context){
        //transform.position = transform.position + new Vector3(0, 150, 0);
        animator.SetBool("isAttacking", true); //Animator Attack check by Omar
        SoundManager.instance.TaroAtaque();
        attackingFun();
    }

    //Damage and hitbox stuff
    void attackingFun()
    {
        isTaroAttacking = true;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Destructible") && isTaroAttacking){
            col.gameObject.GetComponent<CommonEnemy>().Damage(damage);
            isTaroAttacking = false;
        } else if((col.gameObject.tag == "CatBoss") && isTaroAttacking){
            col.gameObject.GetComponent<CatBossIA>().Damage(damage);
            isTaroAttacking = false;
        }
    }

    //Checks is animation ended
    bool AnimatorIsPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
