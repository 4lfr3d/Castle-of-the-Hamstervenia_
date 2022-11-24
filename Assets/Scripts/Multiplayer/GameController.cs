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

    private SaveManager saveHelper; //Carga de juego

    private CoinsManager cm;

    private void Awake(){
        instance = this;

        saveHelper = this.GetComponent<SaveManager>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        saveHelper.Load();

        if(!PhotonNetwork.IsConnected){
            SpawnPlayerOffline();
        } else {
            players = new PM[PhotonNetwork.PlayerList.Length]; //Inicializar el vector de jugadores
            photonView.RPC("InGame", RpcTarget.AllBuffered); //Colocar los players en una posicion de lista de spawner
        }
    }

    void Update(){
        if(cm == null){
            cm = GameObject.Find("Coins").GetComponent<CoinsManager>();
        }
    }

    [PunRPC]
    void InGame(){
        playerInGame++; //Contador de jugadores
        if(playerInGame == PhotonNetwork.PlayerList.Length){
            SpawnPlayer(); //Mandar llamar posiciones de player
        }
    }

    public void preHit(int damage, int viewID){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("HitEnemy", RpcTarget.All, damage, viewID);
    }

    [PunRPC]
    public void HitEnemy(int damage, int viewID){
        CommonEnemy rat = PhotonView.Find(viewID).gameObject.GetComponent<CommonEnemy>();
        rat.lifes = rat.lifes - damage;
        if(rat.lifes <= 0){
            InventorySystem player = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            player.croquetasQty = player.croquetasQty + rat.coinsToAdd;
            player.Update_Ui(); 
            
            cm.coinsToAdd = cm.coinsToAdd + rat.coinsToAdd;
            cm.addCoins();
            PhotonNetwork.Destroy(rat.gameObject);
        }
        rat.gameObject.GetComponent<KnockbackFeedback>().PlayFeedback();
        StartCoroutine(rat.DamageToEnemy(rat.transform.GetChild(0).gameObject));
    }

    void SpawnPlayer(){
        int randomPosition = Random.Range(0, spawnPlayerPositions.Length); //Obtener una posicion random de lista de posiciones
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab, spawnPlayerPositions[randomPosition].position, Quaternion.identity); //Instanciar el player en una posicion aleatoria

        
        PM playScript = playerObj.GetComponent<PM>(); //Obtener script que controla al jugador
        playScript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer); //Mandar ejecutar funcion de inicializador de player
        
    }

    void SpawnPlayerOffline(){
        Instantiate(Resources.Load("Protag"), saveHelper.respawnPoint, Quaternion.identity);
    }

    public void WinGame(){
        Invoke("GoBackToMenu", 1f);
    }

    void GoBackToMenu(){
        Destroy(MultiplayerController.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        MultiplayerController.instance.LoadScene("StartMenu");
    }
    
}
