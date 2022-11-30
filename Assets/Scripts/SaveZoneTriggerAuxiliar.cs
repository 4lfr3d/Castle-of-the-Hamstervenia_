using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZoneTriggerAuxiliar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.gameObject.GetComponent<SaveManager>().isSaveZoneAux = true;
            other.gameObject.GetComponent<SaveManager>().Save();
            other.gameObject.GetComponent<SaveManager>().isSaveZoneAux = false;
        }
    }
}
