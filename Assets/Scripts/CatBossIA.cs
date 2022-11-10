using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBossIA : MonoBehaviour
{
    public int lifes = 2;
    public int coinsToAdd = 100;

    public Animator animator;
    public Transform center;
    public Transform target;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(target.position.x - center.position.x);
        Debug.Log(lifes);

        distance = target.position.x - center.position.x;
        if(distance <= -750 || distance >= 750){
            animator.SetTrigger("Waiting");
        }
        if(distance <= -500 && distance > -750 || distance > 500 && distance < 750){
            animator.SetTrigger("LargeRange");
        }
        if(distance <= -350 && distance > -500){
            animator.SetTrigger("RightHandDown");
        }
        if(distance <= -150 && distance > -350){
            animator.SetTrigger("LeftHandLarge");
        }
        if(distance <= 150 && distance > -150){
            animator.SetTrigger("Center");
        }
        if(distance <= 350 && distance > 150){
            animator.SetTrigger("LeftHandDown");
        }
        if(distance <= 500 && distance > 350){
            animator.SetTrigger("RightHandLarge");
        }
    }


}
