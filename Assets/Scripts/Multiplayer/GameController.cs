using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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

    public Material secondplayer;

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

        if(PhotonNetwork.IsConnected && PhotonNetwork.PlayerList.Length > 1){
            if(PhotonView.Find(2001).gameObject.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material != secondplayer){
                photonView.RPC("Texture", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Texture(){
        PhotonView.Find(2001).gameObject.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = secondplayer;
    }

    [PunRPC]
    void InGame(){
        playerInGame++; //Contador de jugadores
        if(playerInGame == PhotonNetwork.PlayerList.Length){
            SpawnPlayer(); //Mandar llamar posiciones de player
        }
    }

    //Stuff for the CAT BOSS
    public void preHitBoss(int damage, int viewID){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("HitBoss", RpcTarget.All, damage, viewID);
    }

    [PunRPC]
    public void HitBoss(int damage, int viewID){
        CatBossIA cat = PhotonView.Find(viewID).gameObject.GetComponent<CatBossIA>();
        cat.lifes = cat.lifes - damage;
        if(cat.lifes <= 0){
            InventorySystem player = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            player.croquetasQty = player.croquetasQty + cat.coinsToAdd;
            player.Update_Ui(); 
            
            cm.coinsToAdd = cm.coinsToAdd + cat.coinsToAdd;
            cm.addCoins();
            PhotonNetwork.Destroy(cat.gameObject);
        }
        if(cat.lifes <= cat.halflifes){
            cat.transform.position = cat.segundafase.transform.position;
            cat.paredSegundaFase.SetActive(false);
        }

        StartCoroutine(cat.DamageToEnemy(cat.transform.GetChild(0).gameObject));
    }

    //Stuff for the COMMON ENEMIES
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
        if(saveHelper.activeSave.sceneName ==  SceneManager.GetActiveScene().name){
            Instantiate(Resources.Load("Protag"), saveHelper.respawnPoint, Quaternion.identity);        
        }else{
            Instantiate(Resources.Load("Protag"), this.transform.position, Quaternion.identity);
        }
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
