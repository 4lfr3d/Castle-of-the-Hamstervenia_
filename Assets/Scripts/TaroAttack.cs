using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaroAttack : MonoBehaviour
{
    public Animator animator;
    private PlayerInputAction playerInputs;
    public InventorySystem inv;
    public CoinsManager cm;

    private bool isTaroAttacking = false;

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
            Destroy(col.gameObject);
            inv.croquetasQty = inv.croquetasQty + 5;
            cm.coinsToAdd = cm.coinsToAdd + 5;
            cm.addCoins();
            inv.Update_Ui();
            isTaroAttacking = false; 
        }
    }

    //Checks is animation ended
    bool AnimatorIsPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
