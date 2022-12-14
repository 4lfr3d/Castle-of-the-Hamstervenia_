using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CatBossIA : MonoBehaviour
{
    public int lifes = 20;
    public int coinsToAdd = 100;

    public int halflifes;

    public Animator animator;
    public Transform center;
    private Transform target;

    private float distance;

    public Material damageColor;
    public Material enemyMaterial;

    private GameController gameController;

    private CoinsManager cm;

    public GameObject segundafase;
    public GameObject paredSegundaFase;
    public GameObject SaveZone;

    void Awake(){
        halflifes= lifes/2;
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

        if(target == null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(segundafase == null){
            segundafase = GameObject.Find("SegundaFase");
        }
        if(paredSegundaFase == null){
            paredSegundaFase = GameObject.Find("EntradaFase2");
        }
    }

    public void Damage(int damage){
        if(!PhotonNetwork.IsConnected){
            HitEnemyOffline(damage);
        } else{
            //Llamar a la función de GameController para que esa llame a PhotonView (enviar ID)
            PhotonView photonView = PhotonView.Get(this);
            Debug.Log(photonView.ViewID);

            gameController.preHitBoss(damage, photonView.ViewID);

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
            SaveZone.SetActive(true);
            Destroy(this.gameObject);
        }
        if(lifes <= halflifes){
            this.transform.position = segundafase.transform.position;
            paredSegundaFase.SetActive(false);
        }
        StartCoroutine(DamageToEnemy(GameObject.Find("Galleto"))); 
    }

    public IEnumerator DamageToEnemy(GameObject enemy){;
        enemy.GetComponent<Renderer>().material = damageColor;

        yield return new WaitForSeconds(0.25f);

        enemy.GetComponent<Renderer>().material = enemyMaterial;
    }

    void FixedUpdate()
    {
        if(target != null){
            distance = target.position.x - center.position.x;
            if(distance <= -750 || distance >= 750){
                animator.SetTrigger("Waiting");
            }
            if(distance <= -500 && distance > -750 || distance > 500 && distance < 750){
                SoundManager.instance.CatAngry();
                animator.SetTrigger("LargeRange");
            }
            if(distance <= -350 && distance > -500){
                SoundManager.instance.CatAttack();
                animator.SetTrigger("RightHandDown");
            }
            if(distance <= -150 && distance > -350){
                SoundManager.instance.CatAngry();
                animator.SetTrigger("LeftHandLarge");
            }
            if(distance <= 150 && distance > -150){
                SoundManager.instance.CatAttack();
                animator.SetTrigger("Center");
            }
            if(distance <= 350 && distance > 150){
                SoundManager.instance.CatAttack();
                animator.SetTrigger("LeftHandDown");
            }
            if(distance <= 500 && distance > 350){
                SoundManager.instance.CatAngry();
                animator.SetTrigger("RightHandLarge");
            }
        }
    }


}
