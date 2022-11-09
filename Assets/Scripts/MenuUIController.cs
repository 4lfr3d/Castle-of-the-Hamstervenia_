using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Photon.Pun;
//using Photon.Realtime;

public class MenuUIController : MonoBehaviour
{
    //cambiar MonoBehabiour a MonoBehaviourPunCallBacks

    public GameObject lobbywindow;
    public GameObject playerinfoWindow;

    [Header("Menu Datos")]
    public Button createRoomBtn;
    public Button joinRoomBtn;

    [Header("Lobby")]
    public Button startGameBtn;
    public TMP_Text playerTextList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
