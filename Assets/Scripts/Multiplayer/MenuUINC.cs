using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuUINC : MonoBehaviourPunCallbacks
{

    public GameObject mainWindow; //Objeto que contiene ventana principal
    public GameObject lobbyWindow; //Objeto que contiene lobby

    [Header("Menu Principal")]
    public Button createRoomBtn; //Boton para crear sala
    public Button joinRoomBtn;  //Boton para unir sala

    [Header("Lobby")]
    public Button startGameBtn; //Boton iniciar partida
    public TextMeshProUGUI playerTextList; //Texto para imprimir jugadores en room

    void Start(){
        PhotonNetwork.ConnectUsingSettings(); //Realizar conexion con datos colocados en editor
    }

    /*
        <summary>
            Verificar si se establecio la conexion
        </summary>
    */
    public override void OnConnectedToMaster(){
        createRoomBtn.interactable = true; //Activar boton de crear room
        joinRoomBtn.interactable = true; //Activar boton de unirse a un room
        Debug.Log("Connected");
    }

    /*
        <summary>
            Obtener nickname de jugador desde Input
        </summary>
        <param name="_playerName">Input donde se puede editar el nombre</param>
    */
    public void GetPlayerName(TMP_InputField _playerName){
        PhotonNetwork.NickName = _playerName.text; //Asignar el nombre del player en photon
    }

    /*

    */
    public void CreateRoom(TMP_InputField _roomName){
        MultiplayerController.instance.CreateRoom(_roomName.text); //Crear nuevo room desde el Multiplayer controller
    }

    /*
        <summary>
            Permite unirse a un room Existente
        </summary>
    */
    public void JoinRoom(TMP_InputField _roomName){
        MultiplayerController.instance.JoinRoom(_roomName.text); //Crear nuevo room desde el Multiplayer controller
    }

    /*
        <summary>
            Verificar si ya se unio el player a un room
        </summary>
    */
    public override void OnJoinedRoom(){
        lobbyWindow.SetActive(true);
        mainWindow.SetActive(false);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    /*
        <summary>
            Actualizar la informacion dentro de lobby
        </summary>
    */
    [PunRPC]
    public void UpdatePlayerInfo(){
        playerTextList.text=""; //Limpiar campo de texto
        foreach(Player player in PhotonNetwork.PlayerList){
            playerTextList.text += player.NickName + "\n"; //agregar nombre de players
        }
        if(PhotonNetwork.IsMasterClient){
            startGameBtn.interactable=true; //Activar si eres host
        }
        else{
            startGameBtn.interactable=false; //Desactivar si no eres host
        }
    }

    /*
        <summary>
            Salir del room actual
        </summary>
    */
    public void LeaveRoom(){
        lobbyWindow.SetActive(false); //Salir del lobby
        mainWindow.SetActive(true); //Entrar al menu inicial

        MultiplayerController.instance.LeaveRoom(); //Ejecutar funcion de salida
        UpdatePlayerInfo(); //Actualizar informacion
    }

    /*
        <summary>
            Abrir nueva escena con el juego
        </summary>
    */
    public void StartGame(){
        MultiplayerController.instance.photonView.RPC("LoadScene", RpcTarget.All, "SceneGameplay");
    }
}
