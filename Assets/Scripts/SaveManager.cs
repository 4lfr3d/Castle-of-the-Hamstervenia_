using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public bool hasLoaded;

    public Vector3 respawnPoint;
    public TaroHealth healthTaro;
    public InventorySystem inventoryTaro;

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
            if(hasLoaded){
                this.transform.position = activeSave.respawnPoint;
                this.healthTaro.respawnPoint = activeSave.respawnPoint;
                this.healthTaro.numOfSeeds = activeSave.numOfSeeds;
                this.healthTaro.health = activeSave.health;
                this.inventoryTaro.idSaver = activeSave.idEquipableItem;
                this.inventoryTaro.croquetasQty = activeSave.croquetasQty;
                
                for(int i = 0; i<activeSave.infoList.Count; i++){

                    GameObject invObject = GameObject.Find(activeSave.infoList[i].name);
                    InventoryItem inv = new InventoryItem(invObject);

                    inventoryTaro.items.Add(inv);
                    inventoryTaro.items[i].stack = activeSave.infoList[i].stack;
                }

                inventoryTaro.Update_Ui();
                inventoryTaro.EquipedItem(activeSave.idEquipableItem);
            } else{
                activeSave.respawnPoint = this.transform.position;
                activeSave.health = this.healthTaro.health;
                activeSave.numOfSeeds = this.healthTaro.numOfSeeds;
                activeSave.idEquipableItem = this.inventoryTaro.idSaver;
                activeSave.croquetasQty = this.inventoryTaro.croquetasQty;
                activeSave.sceneName =  SceneManager.GetActiveScene().name;
                
                activeSave.infoList.Clear();
            }
        }

    }

    public void Save(){
        activeSave.health = this.healthTaro.health;
        activeSave.numOfSeeds = this.healthTaro.numOfSeeds;
        activeSave.respawnPoint = this.transform.position;
        activeSave.idEquipableItem = this.inventoryTaro.idSaver;
        activeSave.croquetasQty = this.inventoryTaro.croquetasQty;
        this.healthTaro.respawnPoint = activeSave.respawnPoint;

        activeSave.infoList.Clear();

        for(int i = 0; i<inventoryTaro.items.Count; i++){
            InventoryInfo info = new InventoryInfo();

            info.name = inventoryTaro.items[i].obj.name;
            info.stack = inventoryTaro.items[i].stack;

            activeSave.infoList.Add(info);
        }

        //activeSave.itemSave = this.inventoryTaro.items;

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

    public int health, numOfSeeds, idEquipableItem, croquetasQty;

    public string sceneName;

    public List<InventoryInfo> infoList = new List<InventoryInfo>();
}

public class InventoryInfo {
    public string name;
    public int stack;
}
