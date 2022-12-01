using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class TaroHealth : MonoBehaviourPunCallbacks
{
    public int health;
    public int numOfSeeds;

    public Animator anim;
    
    public RawImage[] seeds;
    public Texture2D fullSeed;
    public Texture2D emptySeed;

    private float timeInmmunity;

    public Vector3 respawnPoint;

    public GameObject deathPanel;
    //Canvas group para la pantalla de muerte
    public CanvasGroup deadPanel;

    public Material damageColor;
    private Material taroMaterial;

    void Awake(){
        deathPanel = GameObject.Find("DeathScene");

        for(int i = 0; i < 6; i++){
            seeds[i] = GameObject.Find("HamsterLife" + i.ToString()).GetComponent<RawImage>();
        }
        //Inicializamos Dotween
        DOTween.Init();
    }

    void Start(){
        timeInmmunity = 0f;

        deathPanel.SetActive(false);
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

        if(timeInmmunity > 3f){
            if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "BossHit" || other.gameObject.tag == "RatBoss"){
                if(health == 1){
                    anim.SetTrigger("isDeath");
                    health--;
                    this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    StartCoroutine(DeathTaro());
                }else{
                    StartCoroutine(DamageToTaro());
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(hitVector*150000);
                    anim.SetTrigger("Damaged");
                    SoundManager.instance.TaroHealthLoss();
                    health--;
                    timeInmmunity = 0f;
                }
            }
        }
    }

    //Death
    public IEnumerator DeathTaro(){
        //El fondo del panel de muerte aparece
        deadPanel.DOFade(0f, 0.05f);
        //La imagen que sale cuando mueres
        deathPanel.GetComponent<Image>().DOFade(0f, 0.05f);
        yield return new WaitForSeconds(1);
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn(){
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetTrigger("isRespawned");
        this.transform.position = respawnPoint;
        health = numOfSeeds;
        deathPanel.SetActive(false);
        Time.timeScale = 1f;
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator DamageToTaro(){
        taroMaterial = this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().material;
        this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().material = damageColor;

        yield return new WaitForSeconds(0.25f);

        this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().material = taroMaterial;
    }
}
