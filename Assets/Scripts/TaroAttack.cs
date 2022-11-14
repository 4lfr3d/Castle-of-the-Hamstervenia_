using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaroAttack : MonoBehaviour
{
    // Cuca 3 Rata 5
    public Animator animator;
    private PlayerInputAction playerInputs;
    public InventorySystem inv;
    public int damage = 1;
    public Material damageColor;

    private bool isTaroAttacking = false;
    private Material enemyMaterial;
    private CoinsManager cm;

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
        if(cm == null){
            cm = GameObject.Find("Coins").GetComponent<CoinsManager>();
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
            col.gameObject.GetComponent<CommonEnemy>().lifes = col.gameObject.GetComponent<CommonEnemy>().lifes - damage;
            if(col.gameObject.GetComponent<CommonEnemy>().lifes <= 0){
                inv.croquetasQty = inv.croquetasQty + col.gameObject.GetComponent<CommonEnemy>().coinsToAdd;
                cm.coinsToAdd = cm.coinsToAdd + col.gameObject.GetComponent<CommonEnemy>().coinsToAdd;
                cm.addCoins();
                inv.Update_Ui();
                Destroy(col.gameObject);
            }
            StartCoroutine(DamageToEnemy(col.transform.GetChild(0).gameObject)); 
            Vector2 hitVector = (col.transform.position - transform.position).normalized;
            hitVector.y = 0;
            hitVector = hitVector.normalized;
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(hitVector * 2500000);
            isTaroAttacking = false;
        } else if((col.gameObject.tag == "CatBoss") && isTaroAttacking){
            col.gameObject.transform.parent.parent.gameObject.GetComponent<CatBossIA>().lifes = col.gameObject.transform.parent.parent.gameObject.GetComponent<CatBossIA>().lifes - damage;
            col.gameObject.transform.parent.parent.gameObject.GetComponent<CatBossIA>().animator.SetTrigger("Hit");
            if(col.gameObject.transform.parent.parent.gameObject.GetComponent<CatBossIA>().lifes <= 0){
                Destroy(col.gameObject.transform.parent.parent.gameObject);
                inv.croquetasQty = inv.croquetasQty + col.gameObject.transform.parent.parent.gameObject.GetComponent<CatBossIA>().coinsToAdd;
                cm.coinsToAdd = cm.coinsToAdd + col.gameObject.GetComponent<CatBossIA>().coinsToAdd;
                cm.addCoins();
                inv.Update_Ui();
            }
            isTaroAttacking = false;
        }
    }

    //Checks is animation ended
    bool AnimatorIsPlaying(){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    IEnumerator DamageToEnemy(GameObject enemy){
        enemyMaterial = enemy.GetComponent<Renderer>().material;
        enemy.GetComponent<Renderer>().material = damageColor;

        yield return new WaitForSeconds(0.25f);

        enemy.GetComponent<Renderer>().material = enemyMaterial;
    }
}
