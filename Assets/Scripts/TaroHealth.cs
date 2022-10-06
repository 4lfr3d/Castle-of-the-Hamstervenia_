using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaroHealth : MonoBehaviour
{
    public int health;
    public int numOfSeeds;

    public Animator anim;
    
    public RawImage[] seeds;
    public Texture2D fullSeed;
    public Texture2D emptySeed;

    private float timeInmmunity;

    public Transform respawnPoint;

    public GameObject deathPanel;

    void Start(){
        timeInmmunity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeInmmunity += Time.deltaTime;

        if(health > numOfSeeds){
            health = numOfSeeds;
        }

        for(int i=0; i<seeds.Length; i++){
            if(i < health){
                seeds[i].texture = fullSeed;
            } else{
                seeds[i].texture = emptySeed;
            }

            if(i < numOfSeeds){
                seeds[i].enabled = true;
            } else{
                seeds[i].enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Vector2 hitVector = (transform.position - other.transform.position).normalized;
        hitVector = (transform.position - other.transform.position);
        hitVector.y = 0;
        hitVector = hitVector.normalized;

        Debug.Log(hitVector);

        if(timeInmmunity > 3f){
            if(other.gameObject.tag == "Enemy"){
                if(health == 1){
                    anim.SetTrigger("isDeath");
                    health--;
                    this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    StartCoroutine(DeathTaro());
                }else{
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(hitVector*150000);
                    anim.SetTrigger("Damaged");
                    health--;
                    timeInmmunity = 0f;
                }
            }
        }
    }

    //Death
    public IEnumerator DeathTaro(){
        yield return new WaitForSeconds(1);
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn(){
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetTrigger("isRespawned");
        this.transform.position = respawnPoint.transform.position;
        health = numOfSeeds;
        deathPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
