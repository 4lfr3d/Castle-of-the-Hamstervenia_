using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CatBossIA : MonoBehaviour
{
    public int lifes = 2;
    public int coinsToAdd = 100;

    public Animator animator;
    public Transform center;
    private Transform target;

    private float distance;

    public Material damageColor;
    private Material enemyMaterial;

    private GameController gameController;

    private CoinsManager cm;


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
        StartCoroutine(DamageToEnemy(this.transform.GetChild(0).gameObject)); 
    }

    public IEnumerator DamageToEnemy(GameObject enemy){
        enemyMaterial = enemy.GetComponent<Renderer>().material;
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
