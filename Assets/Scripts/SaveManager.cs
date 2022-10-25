using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public bool hasLoaded;

    public Vector3 respawnPoint;
    public int health, numOfSeeds;
    public InventorySystem inventorySystem, inventoryLoaded;
    public TaroHealth healthTaro;

    private bool saveZoneTrigger;

    private PlayerInputAction playerInputs;

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += SaveInput;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    void SaveInput(InputAction.CallbackContext context){
        if(saveZoneTrigger){
            Save();
        }
    }

    private void Awake(){
        playerInputs = new PlayerInputAction();

        Load();
    }

    void Start()
    {
        if(gameObject.tag == "Player"){
            activeSave.respawnPoint = this.transform.position;
            if(hasLoaded){
                respawnPoint = activeSave.respawnPoint;
                health = activeSave.health;
                numOfSeeds = activeSave.numOfSeeds;

                this.transform.position = respawnPoint;
                this.healthTaro.numOfSeeds = numOfSeeds;
                this.healthTaro.health = health;
            } else{
                activeSave.health = this.healthTaro.health;
                activeSave.numOfSeeds = this.healthTaro.numOfSeeds;
            }
        }

    }

    public void Save(){
        activeSave.health = this.healthTaro.health;
        activeSave.numOfSeeds = this.healthTaro.numOfSeeds;
        activeSave.respawnPoint = this.transform.position;

        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/mysave1.save", FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Saved");
    }

    public void Load(){
        string dataPath = Application.persistentDataPath;

        if(System.IO.File.Exists(dataPath + "/mysave1.save")){
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/mysave1.save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Loaded");

            hasLoaded = true;
        } else {
            hasLoaded = false;
        }
    }

    public bool LoadChecker(){
        if(hasLoaded) return true; else return false;
    }

    public void DeleteSaveData(){
        string dataPath = Application.persistentDataPath;

        if(System.IO.File.Exists(dataPath + "/mysave1.save")){
            File.Delete(dataPath + "/mysave1.save");

        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "SaveZone"){
            saveZoneTrigger = true;
        } else{
            saveZoneTrigger = false;
        }
    }

}

[System.Serializable]
public class SaveData {

    public Vector3 respawnPoint;

    public int health, numOfSeeds;

}
