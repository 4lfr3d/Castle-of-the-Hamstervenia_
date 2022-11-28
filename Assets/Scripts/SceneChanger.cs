using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChanger : MonoBehaviourPunCallbacks
{
    public string NextScene;
    public GameObject taro;
    public GameObject respawnIn;
    public GameObject respawnOut;

    void Awake(){
        taro = GameObject.Find("Protag");
        respawnIn = GameObject.Find("ChangeRespawnIn").GetComponent<Transform>();
        respawnIn = GameObject.Find("ChangeRespawnOut").GetComponent<Transform>();
    }

    void Start(){
        PlayerPrefs.SetString("LoadScreenNextScene", NextScene);
        PlayerPrefs.SetFloat("LoadScreenDelay", 2f);
    }

    public void ChangeScene(string changePoint){
        if(!PhotonNetwork.IsConnected){
            if(changePoint == "ChangePointIn"){
                taro.GetComponent<SaveManager>().respawnPoint = respawnIn.position;
            }
            else{
                taro.GetComponent<SaveManager>().respawnPoint = respawnOut.position;
            }
            SceneManager.LoadScene("LoadScene");
        }
        else{
<<<<<<< Updated upstream
            MultiplayerController.instance.photonView.RPC("LoadScene", RpcTarget.All, NextScene);
            PM[] players = new PM[PhotonNetwork.PlayerList.Length];
            photonView.RPC("InGame", RpcTarget.AllBuffered);
=======
            MultiplayerController.instance.photonView.RPC("LoadScene", RpcTarget.All, "NextScene");
>>>>>>> Stashed changes
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.collider.CompareTag("Player")){
            ChangeScene(other.gameObject.name);
        }
    }
}
