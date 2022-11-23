using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float fuerza = 16, delay = 0.25f;
    public UnityEvent OnBegin, OnDone;

    private GameObject protag;

    void Update(){
        if(protag == null){
            protag = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void PlayFeedback(){
        //StopAllCoroutines();
        OnBegin?.Invoke();
        Vector3 direccion = (transform.position - protag.transform.position).normalized;
        rb2d.AddForce(direccion * fuerza, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset(){
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
