using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boringTaro : StateMachineBehaviour
{
    private float idleTime = 0f;

    public float timeUntilBored = 5f;

    private int boredAnimation;

    private bool isBored;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime = 0f;
        isBored = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime += Time.deltaTime;
        //Debug.Log(idleTime);

        if(idleTime > timeUntilBored){
            isBored = true;

            boredAnimation = Random.Range(1, 4);

            Debug.Log(boredAnimation);
            
            idleTime = 0f;
        }

        
        if(isBored){
            if(boredAnimation == 1){
                isBored = false;
                animator.SetTrigger("bored1");
            }
            else if(boredAnimation == 2){
                isBored = false;
                animator.SetTrigger("bored2");
            }
            else if(boredAnimation == 3){
                isBored = false;
                animator.SetTrigger("bored3");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
