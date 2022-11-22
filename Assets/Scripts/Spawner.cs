using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Spawner : MonoBehaviourPunCallbacks
{

    public PM[] players; //controlador de player

    void Awake(){
        if(!PhotonNetwork.IsConnected){
            Instantiate(Resources.Load("Protag"), this.transform.position, Quaternion.identity);
        }
        else{
            players = new PM[PhotonNetwork.PlayerList.Length]; //Inicializar el vector de jugadores
            photonView.RPC("InGame", RpcTarget.AllBuffered); //Colocar los players en una posicion de lista de spawner
        }
    }

}
