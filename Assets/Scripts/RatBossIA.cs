using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class RatBossIA : MonoBehaviour
{
    public int lifes = 15;
    public int coinsToAdd = 100;

    private string currentState = "IdleState";
    public float chaseRange = 300;
    public float attackRange = 50;
    private NavMeshAgent agent;

    public Animator animator;
    private Transform target;

    private float distance;

    public Material damageColor;
    public Material enemyMaterial;

    private GameController gameController;

    private CoinsManager cm;

    public GameObject unlockablePath;
    public GameObject afterBossPath;

    void Start(){
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cm == null){
            cm = GameObject.Find("Coins").GetComponent<CoinsManager>();
        }
        if(gameController == null){
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
        if(unlockablePath == null){
            unlockablePath = GameObject.Find("UnlockablePath");
        }
        if(afterBossPath == null){
            afterBossPath = GameObject.Find("AfterBossWall");
        }
    }

    public void Damage(int damage){
        if(!PhotonNetwork.IsConnected){
            HitEnemyOffline(damage);
        } else{
            //Llamar a la función de GameController para que esa llame a PhotonView (enviar ID)
            PhotonView photonView = PhotonView.Get(this);
            Debug.Log(photonView.ViewID);

            gameController.preHitBossRat(damage, photonView.ViewID);

            //photonView.RPC("HitEnemy", RpcTarget.All, damage);
        }
    }

    public void HitEnemyOffline(int damage){
        lifes = lifes - damage;
        if(lifes <= 0){
            InventorySystem player = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            player.croquetasQty = player.croquetasQty + coinsToAdd;
            player.Update_Ui();
            cm.coinsToAdd = cm.coinsToAdd + coinsToAdd;
            cm.addCoins();
            Destroy(this.gameObject);
            unlockablePath.SetActive(true);
            afterBossPath.SetActive(false);
        }
        StartCoroutine(DamageToEnemy(this.transform.GetChild(0).GetChild(1).gameObject)); 
    }

    public IEnumerator DamageToEnemy(GameObject enemy){
        enemy.GetComponent<Renderer>().material = damageColor;

        yield return new WaitForSeconds(0.25f);

        enemy.GetComponent<Renderer>().material = enemyMaterial;
    }

    void FixedUpdate()
    {
        if(target == null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        float distance = Vector3.Distance(transform.position, target.position);

        if(currentState == "IdleState"){
            if(distance < chaseRange){
                currentState = "ChaseState";
                animator.SetBool("isIdle", false);
                agent.isStopped = false;
            }
        } else if(currentState == "ChaseState"){
            agent.destination = target.position;
            animator.SetTrigger("chase");
            animator.SetBool("isAttacking", false);
            if(distance > chaseRange){
                currentState = "IdleState";
                animator.SetBool("isIdle", true);
                agent.isStopped = true;
            }
            if(distance < attackRange){
                currentState = "AttackState";
            }
            if(target.position.x > transform.position.x){
                //Move right
                transform.rotation = Quaternion.Euler(0,180,0);
            } else{
                //Move left
                transform.rotation = Quaternion.Euler(0,0,0);
            }
        } else if(currentState == "AttackState"){
            agent.destination = target.position;
            animator.SetBool("isAttacking", true);
            SoundManager.instance.RatonAtaque();
            if(distance > attackRange){
                currentState = "ChaseState";
            }
        }
    }


}
