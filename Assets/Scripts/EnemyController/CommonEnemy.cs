/*
    CODE BY GDTitans - https://www.youtube.com/watch?v=WluxMKsL1o4&list=PL0WgRP7BtOexkgEO9lOVubYnagjjbU2CI&index=12
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemy : MonoBehaviour
{
    private string currentState = "IdleState";
    private Transform target;
    public float chaseRange = 300;
    public float attackRange = 50;
    public float speed = 50;
    public int lifes;
    public int coinsToAdd;

    public string enemyType;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        //r for Rat, c for cockroach
        if(enemyType == "r"){
            if(currentState == "IdleState"){
                if(distance < chaseRange) currentState = "ChaseState";
            } else if(currentState == "ChaseState"){
                animator.SetTrigger("chase");
                animator.SetBool("isAttacking", false);

                if(distance<attackRange){
                    currentState = "AttackState";
                }

                if(target.position.x > transform.position.x){
                    //Move right
                    transform.Translate(transform.right*speed*Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0,180,0);
                } else{
                    //Move left
                    transform.Translate(-transform.right*speed*Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0,0,0);
                }
            } else if(currentState == "AttackState"){
                animator.SetBool("isAttacking", true);
                SoundManager.instance.RatonAtaque();
                if(distance>attackRange){
                    currentState = "ChaseState";
                }
            }
        } else if(enemyType == "c"){
            if(distance < chaseRange){
                if(target.position.x > transform.position.x){
                    //Move right
                    transform.Translate(transform.right*speed*Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0,0,0);
                } else{
                    //Move left
                    transform.Translate(-transform.right*speed*Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0,180,0);
                }
            }
        }
    }
}
