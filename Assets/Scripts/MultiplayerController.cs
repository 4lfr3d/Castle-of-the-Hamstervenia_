using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    //Singleton
    public static MultiplayerController instance;

    private void Awake(){
        if(instance != null && instance != this){
            gameObject.SetActive(false);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Realizar conexion con datos colocados en editor
    }

    /*
        <summary>
            Crear un nuevo room con el nombre dado
        </summary>
        <param name="_name"> Nombre que va tener el room </param>
    */
    public void CreateRoom(string _name){
        PhotonNetwork.CreateRoom(_name);
    }

    /*
        <summary>
            Verificacion si el room fue creado
        </summary>
    */
    public override void OnCreatedRoom(){
        Debug.Log("Se creo room: " + PhotonNetwork.CurrentRoom.Name);
    }

    /*
        <summary>
            Abandonar el room actual desde lobby
        </summary>
    */
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    /*
        <summary>
            Cargar una nueva escena usando Photon
        </summary>
        <param name="_nameScene">Nombre de la escena que se va a cargar</param>
    */
    [PunRPC]
    public void LoadScene(string _nameScene){
        PhotonNetwork.LoadLevel(_nameScene);
    }
}
