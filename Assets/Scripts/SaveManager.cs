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
using DG.Tweening;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public bool hasLoaded;

    public Vector3 respawnPoint;
    public TaroHealth healthTaro;
    public InventorySystem inventoryTaro;
    public Store storeTaro;
    public Forge forgeTaro;
    public TaroAttack attackTaro;

    public Animator animationSaver;
    public TMP_Text textSaver;

    private string[] phraseSaver = {"Seedable", "Bowl-ing", "Bowl Lit", "I seed a bowl"};

    private bool saveZoneTrigger;
    
    public GameObject interactionButton;

    private PlayerInputAction playerInputs;

    [Header("Save Options Interaction")]
    public CanvasGroup saveOptions_Panel;
    public GameObject saveOptions_GB;

    private Transform guardar_boton;
    private Transform salirPartida_boton;
    private Transform cancelar_boton;

    private float scaleTime = 0.25f;
    private float fadeTime = 0.5f;

    private Vector3 objetoScalaNormal = new Vector3(1.3359f,1.403096f,1.3359f);
    private Vector3 objetoScalaReducido = Vector3.zero;

    public bool isSaveZoneAux = false;



    private void Awake(){
        if(this.tag == "Player"){
            animationSaver = GameObject.Find("SaveAdvisor").GetComponent<Animator>();
            textSaver = GameObject.Find("SaveAdvisor").GetComponent<TMP_Text>();
            saveOptions_Panel = GameObject.Find("SaveOptions").GetComponent<CanvasGroup>();

            saveOptions_GB = GameObject.Find("SaveOptions");
            interactionButton = GameObject.Find("InteractSaveZone");


            guardar_boton = GameObject.Find("Guardar").GetComponent<Transform>();
            salirPartida_boton = GameObject.Find("Salir").GetComponent<Transform>();
            cancelar_boton = GameObject.Find("CancelarChat").GetComponent<Transform>();

            DOTween.Init();

            /* Está desvaneciendo el panel a 0 de opacidad durante 0 segundos. */
            saveOptions_Panel.DOFade(0f,0f);

            ScaleDownSaveOptions();
        }

        playerInputs = new PlayerInputAction();

        Load();
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += SaveInput;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    void SaveInput(InputAction.CallbackContext context){
        if(saveZoneTrigger){
            saveOptions_GB.SetActive(true);
            /* Descaneciendo el panel a 1 de opacidad durante 0,5 segundos y luego llamando a la función ScaleUpSaveOptions. */
            SoundManager.instance.WaterDropSFX();
            saveOptions_Panel.DOFade(1f,fadeTime).OnComplete(ScaleUpSaveOptions);
        }
    }

    public void ScaleUpSaveOptions(){
        /* Es una secuencia que escala los botones a su tamaño predeterminado, uno tras otro. Cada
        botón realiza una curva de animación de rebote después de escalar. */
        Sequence escalarArriba = DOTween.Sequence();
        escalarArriba.Append(guardar_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
        escalarArriba.Append(salirPartida_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
        escalarArriba.Append(cancelar_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
    }

/* Reduce todos los botones en las interacciones al mismo tiempo, después de completar eso, desactiva la interacción del panel. */
    public void ScaleDownSaveOptions(){
        Sequence escalarAbajo = DOTween.Sequence();
        escalarAbajo.Join(guardar_boton.DOScale(objetoScalaReducido, 0f));
        escalarAbajo.Join(salirPartida_boton.DOScale(objetoScalaReducido, 0f));
        /* Reduciendo la escala del botón y luego llamando a la función Activate_Panel con un parámetro falso. */
        escalarAbajo.Join(cancelar_boton.DOScale(objetoScalaReducido, 0f).OnComplete(()=>Activate_Panel(false)));
    }

    public void OpcionGuardadoElegida(){
        /* Una secuencia que desvanece la imagen del panel, luego reduce cada botón a Vector3.zero uno
        tras otro. Finalmente, después de escalar el último botón, se llama a la función
        Activate_Panel con un parámetro falso. */
        Sequence opcionelegida = DOTween.Sequence();
        opcionelegida.Join(saveOptions_Panel.DOFade(0,0.5f));
        opcionelegida.Append(guardar_boton.DOScale(objetoScalaReducido, 0f).SetDelay(1f));
        opcionelegida.Append(salirPartida_boton.DOScale(objetoScalaReducido, 0f));
        opcionelegida.Append(cancelar_boton.DOScale(objetoScalaReducido, 0f).OnComplete(()=>Activate_Panel(false)));
    }

    public void Activate_Panel(bool activate){
        saveOptions_GB.SetActive(activate);
        SoundManager.instance.StopWaterDrop();
    }


    void Start()
    {
        if(gameObject.tag == "Player"){
            textSaver.text = phraseSaver[Random.Range(0, phraseSaver.Length)];
            if(hasLoaded){
                if(activeSave.sceneName ==  SceneManager.GetActiveScene().name){
                    this.transform.position = activeSave.respawnPoint;
                    this.healthTaro.respawnPoint = activeSave.respawnPoint;
                }
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

                this.attackTaro.damage = activeSave.needleDamage;
                this.forgeTaro.weapons[0].cost = activeSave.forgeCost;
                this.forgeTaro.weapons[0].actualLevel = activeSave.forgeActualLevel;

                for(int i = 0; i<activeSave.infoStore.Count; i++){
                    this.storeTaro.items[i].obj.name = activeSave.infoStore[i].name;
                    this.storeTaro.items[i].qty = activeSave.infoStore[i].qty;
                }

                storeTaro.UpdateStore();
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

        activeSave.infoStore.Clear();

        for(int i = 0; i < 3; i++){
            StoreSaveInfo info = new StoreSaveInfo();

            info.name = storeTaro.items[i].obj.name;
            info.qty = storeTaro.items[i].qty;

            activeSave.infoStore.Add(info);
        }

        activeSave.needleDamage = this.attackTaro.damage;
        activeSave.forgeCost = this.forgeTaro.weapons[0].cost;
        activeSave.forgeActualLevel = this.forgeTaro.weapons[0].actualLevel;
        activeSave.sceneName =  SceneManager.GetActiveScene().name;

        //activeSave.itemSave = this.inventoryTaro.items;

        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/mysave1.save", FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Saved");
        
        if(!isSaveZoneAux){
            textSaver.text = phraseSaver[Random.Range(0, phraseSaver.Length)];
            animationSaver.SetTrigger("Saver");
        }
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
            SoundManager.instance.WaterDropSFX();
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

    public int health, numOfSeeds, idEquipableItem, croquetasQty, needleDamage, forgeCost, forgeActualLevel;

    public string sceneName;

    public List<InventoryInfo> infoList = new List<InventoryInfo>();

    public List<StoreSaveInfo> infoStore = new List<StoreSaveInfo>();
}

public class InventoryInfo {
    public string name;
    public int stack;
}

public class StoreSaveInfo {
    public string name;
    public int qty;
}
