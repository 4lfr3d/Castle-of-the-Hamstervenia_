using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChanger : MonoBehaviourPunCallbacks
{
    public string NextScene;

    void Start(){
        PlayerPrefs.SetString("LoadScreenNextScene", NextScene);
        PlayerPrefs.SetFloat("LoadScreenDelay", 2f);
    }

    public void ChangeScene(){
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("LoadScene");
        }
        else{
            MultiplayerController.instance.photonView.RPC("LoadScene", RpcTarget.All, NextScene);
            PM[] players = new PM[PhotonNetwork.PlayerList.Length];
            photonView.RPC("InGame", RpcTarget.AllBuffered);
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.collider.CompareTag("Player")){
            ChangeScene();
        }
    }
}
