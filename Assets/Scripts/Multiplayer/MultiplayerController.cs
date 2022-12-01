using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    //Singleton
    public static MultiplayerController instance;

    private void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogError("Iniciar Connection");
        //PhotonNetwork.ConnectUsingSettings(); //Realizar conexion con datos colocados en editor
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

    // <summary>
    // Unirse a room ya existente
    // </summary>
    // <param name="_name">Nombre del room al que se quiere unir</param>
    public void JoinRoom(string _name)
    {
        Debug.Log(_name);
        PhotonNetwork.JoinRoom(_name);
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
        SceneManager.LoadScene(_nameScene);
    }
}
