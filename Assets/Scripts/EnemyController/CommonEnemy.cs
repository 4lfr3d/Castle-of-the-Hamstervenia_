/*
    CODE BY GDTitans - https://www.youtube.com/watch?v=WluxMKsL1o4&list=PL0WgRP7BtOexkgEO9lOVubYnagjjbU2CI&index=12
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class CommonEnemy : MonoBehaviour
{
    public float chaseRange = 300;
    public float attackRange = 50;
    public int lifes;
    public int coinsToAdd;
    public string enemyType;
    public Animator animator;

    private string currentState = "IdleState";
    private NavMeshAgent agent;
    private Transform target;
    
    public Material damageColor;
    private Material enemyMaterial;

    private GameController gameController;

    private CoinsManager cm;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        if(cm == null){
            cm = GameObject.Find("Coins").GetComponent<CoinsManager>();
        }
        if(gameController == null){
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    public void Damage(int damage){
        if(!PhotonNetwork.IsConnected){
            HitEnemyOffline(damage);
        } else{
            //Llamar a la función de GameController para que esa llame a PhotonView (enviar ID)
            PhotonView photonView = PhotonView.Get(this);
            Debug.Log(photonView.ViewID);

            gameController.preHit(damage, photonView.ViewID);

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
        }
        this.gameObject.GetComponent<KnockbackFeedback>().PlayFeedback();
        StartCoroutine(DamageToEnemy(this.transform.GetChild(0).gameObject)); 
    }

    /*[PunRPC]
    public void HitEnemy(int damage, int viewID){
        lifes = lifes - damage;
        if(lifes <= 0){
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                InventorySystem player = p.GetComponent<InventorySystem>();
                player.croquetasQty = player.croquetasQty + coinsToAdd;
                player.Update_Ui();   
            }
            cm.coinsToAdd = cm.coinsToAdd + coinsToAdd;
            cm.addCoins();
            PhotonNetwork.Destroy(this.gameObject);
        }
        this.gameObject.GetComponent<KnockbackFeedback>().PlayFeedback();
        StartCoroutine(DamageToEnemy(this.transform.GetChild(0).gameObject));
    }*/

    public IEnumerator DamageToEnemy(GameObject enemy){
        enemyMaterial = enemy.GetComponent<Renderer>().material;
        enemy.GetComponent<Renderer>().material = damageColor;

        yield return new WaitForSeconds(0.25f);

        enemy.GetComponent<Renderer>().material = enemyMaterial;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        float distance = Vector3.Distance(transform.position, target.position);

        //r for Rat, c for cockroach
        if(enemyType == "r"){
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
        } else if(enemyType == "c"){
            if(distance < chaseRange){
                agent.isStopped = false;
                agent.destination = target.position;

                if(target.position.x > transform.position.x){
                    //Move right
                    transform.rotation = Quaternion.Euler(0,0,0);
                } else{
                    //Move left
                    transform.rotation = Quaternion.Euler(0,180,0);
                }
            }
            else{
                agent.isStopped = true;
            }
        }
    }
}
