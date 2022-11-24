using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChanger : MonoBehaviourPunCallbacks
{
    public string NextScene;

    public void ChangeScene(string NextScene){
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene(NextScene);
        }
        else{
            MultiplayerController.instance.photonView.RPC("LoadScene", RpcTarget.All, NextScene);
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.collider.CompareTag("Player")){
            ChangeScene(NextScene);
        }
    }
}
