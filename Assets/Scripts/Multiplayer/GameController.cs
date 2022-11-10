using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{
    //Instancias
    public static GameController instance;

    public bool isGameEnd = false; //Saber si el juego termino
    public string playerPrefab; //Prefab de la carpeta de recursos

    public Transform[] spawnPlayerPositions; //Posiciones donde se puede colocar los players
    public PM[] players; //controlador de player

    private int playerInGame; //Numero de players en el room

    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new PM[PhotonNetwork.PlayerList.Length]; //Inicializar el vector de jugadores
        photonView.RPC("InGame", RpcTarget.AllBuffered); //Colocar los players en una posicion de lista de spawner
    }

    [PunRPC]
    void InGame(){
        playerInGame++; //Contador de jugadores
        if(playerInGame == PhotonNetwork.PlayerList.Length){
            SpawnPlayer(); //Mandar llamar posiciones de player
        }
    }

    void SpawnPlayer(){
        int randomPosition = Random.Range(0, spawnPlayerPositions.Length); //Obtener una posicion random de lista de posiciones
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab, spawnPlayerPositions[randomPosition].position, Quaternion.identity); //Instanciar el player en una posicion aleatoria

        
        PM playScript = playerObj.GetComponent<PM>(); //Obtener script que controla al jugador
        playScript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer); //Mandar ejecutar funcion de inicializador de player
        
    }

}
