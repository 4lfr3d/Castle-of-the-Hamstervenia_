/*
    CODE BY GDTitans - https://www.youtube.com/watch?v=WluxMKsL1o4&list=PL0WgRP7BtOexkgEO9lOVubYnagjjbU2CI&index=12
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Awake(){
        target = GameObject.Find("Protag").transform;
    }

    // Update is called once per frame
    void Update()
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
                    agent.isStopped = false;
                    agent.destination = target.position;
                }
                else{
                    agent.isStopped = true;
                }
            } else if(currentState == "ChaseState"){
                animator.SetTrigger("chase");
                animator.SetBool("isAttacking", false);

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
