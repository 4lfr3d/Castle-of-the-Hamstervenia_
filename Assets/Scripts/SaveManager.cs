using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public bool hasLoaded;

    public Vector3 respawnPoint;
    public TaroHealth healthTaro;
    public InventorySystem inventoryTaro;

    public Animator animationSaver;
    public TMP_Text textSaver;

    private string[] phraseSaver = {"Seedable", "Bowl-ing", "Bowl Lit", "I seed a bowl"};

    private bool saveZoneTrigger;
    
    public GameObject interactionButton;

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
        textSaver.text = phraseSaver[Random.Range(0, phraseSaver.Length)];
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

                    if(invObject.name == "Bendicion") invObject.SetActive(false);

                    inventoryTaro.items.Add(inv);
                    inventoryTaro.items[i].stack = activeSave.infoList[i].stack;
                }

                inventoryTaro.Update_Ui();
                inventoryTaro.EquipedItem(activeSave.idEquipableItem);
                inventoryTaro.StartInventory();
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
        
        textSaver.text = phraseSaver[Random.Range(0, phraseSaver.Length)];
    
        animationSaver.SetTrigger("Saver");
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
            interactionButton.gameObject.transform.position = other.gameObject.transform.position + new Vector3(70, 5, 0);
            interactionButton.SetActive(true);
        } else{
            saveZoneTrigger = false;
            interactionButton.SetActive(false);
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
