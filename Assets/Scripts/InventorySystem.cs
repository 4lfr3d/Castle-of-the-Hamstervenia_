using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject ui_Window;
    public GameObject ui_Description_window;
    public GameObject consume_item;
    public Image[] items_images;
    public TMP_Text[] items_counters;
    public Image description_image;
    public Image hud_item;
    public TMP_Text description_Title;
    public TMP_Text item_description;

    public Image equipedItemImg;
    public TMP_Text equipedItemQty;
    public TMP_Text idItem;
    public int idSaver = -1;

    public TMP_Text croquetasQtyTxt;
    public int croquetasQty = 0;

    private PlayerInputAction playerInputs;
    public CoinsManager cm;

    public void Awake(){
        ui_Window = GameObject.Find("Inventario");
        ui_Description_window = GameObject.Find("Info");
        
        for(int i = 0; i < 6; i++){
            items_images[i] = GameObject.Find("InventorySlot" + i.ToString()).transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
            items_counters[i] = GameObject.Find("InventorySlot" + i.ToString()).transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        }

        description_image = GameObject.Find("Description").GetComponent<Image>();
        hud_item = GameObject.Find("ConsumableItem").GetComponent<Image>();
        description_Title = GameObject.Find("Infotitle").GetComponent<TMP_Text>();
        item_description = GameObject.Find("ItemDescription").GetComponent<TMP_Text>();
        equipedItemImg = GameObject.Find("ConsumableItem").GetComponent<Image>();
        equipedItemQty = GameObject.Find("ConsumableText").GetComponent<TMP_Text>();
        idItem = GameObject.Find("idHolderInvisible").GetComponent<TMP_Text>();
        croquetasQtyTxt = GameObject.Find("CroquetasQty").GetComponent<TMP_Text>();
        cm = GameObject.Find("Coins").GetComponent<CoinsManager>();

        playerInputs = new PlayerInputAction();
    }

    void Update(){
    }

    private void OnEnable(){
        playerInputs.Player.CONSUMETHECHILD.performed += DoConsume;
        playerInputs.Player.CONSUMETHECHILD.Enable();


        /*playerInputs.Menú.Selected.performed += DoSelected;
        playerInputs.Menú.Selected.Enable();*/
    }

    private void OnDisable(){
        playerInputs.Player.CONSUMETHECHILD.Disable();

        playerInputs.Menú.Selected.Disable();
    }

    public void PickUp(GameObject item){

        if(item.GetComponent<Item>().stackable){
            //check if existing item, yes stack, no add
            InventoryItem exisitingItem = items.Find(x => x.obj.name == item.name);
            if(item.name == "Croquetas"){
                croquetasQty++;
                cm.coinsToAdd++;
                cm.addCoins();
                croquetasQtyTxt.text = croquetasQty.ToString();
            }
            else if(exisitingItem != null){
                bool resultParse;
                int numberParse;
                resultParse = int.TryParse(idItem.text, out numberParse);
                exisitingItem.stack++;
                item.GetComponent<Item>().count++;
                if(resultParse){
                    if(items.IndexOf(exisitingItem) == numberParse){
                        equipedItemQty.text = exisitingItem.stack.ToString();
                    }
                }
            }
            else{
                InventoryItem inv = new InventoryItem(item);
                items.Add(inv);   
            }
        }
        else{
            InventoryItem inv = new InventoryItem(item);
            items.Add(inv);
        }

        if(item.name != "Croquetas"){
            Update_Ui();
        }
    }

    public bool CanPickUp(){
        if(items.Count >= items_images.Length){
            return false;
        } else{
            return true;
        }
    }


    public void Update_Ui(){

        HideAll();
        
        for (int i = 0; i < items.Count; i++)
        {
            items_images[i].sprite = items[i].obj.GetComponent<SpriteRenderer>().sprite;
            if(items[i].stack == 1){
                items_counters[i].text = " ";
            }
            else{
                items_counters[i].text = items[i].stack.ToString();
            }
            items_counters[i].gameObject.SetActive(true);
            items_images[i].gameObject.SetActive(true);
        }

        croquetasQtyTxt.text = croquetasQty.ToString();
    }

    void HideAll(){
        foreach (var item in items_images)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var counter in items_counters)
        {
            counter.gameObject.SetActive(false);
        }

        HideDescription();
    }

    //mostrar info al hacer hover en inventario

    public void ShowDescription(int id){
        description_image.sprite = items_images[id].sprite;

        if(items[id].stack == 1)
        {
            description_Title.text = items[id].obj.name;
        }
        else
        {
            items_counters[id].text = items[id].stack.ToString();
            description_Title.text = items[id].obj.name + " x" + items[id].stack;
        }

        item_description.text = items[id].obj.GetComponent<Item>().item_desc;


        description_image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        item_description.gameObject.SetActive(true);
        

    }

//ocultar informacion descriptcion
    public void HideDescription(){
        description_image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        item_description.gameObject.SetActive(false);
    }

    public void DoConsume(InputAction.CallbackContext context){
        //Consume(int.Parse(idItem.text));
        bool resultParse;
        int numberParse;
        resultParse = int.TryParse(idItem.text, out numberParse);
        if(resultParse){
            Consume(numberParse);
        }
    }

    public void DoSelected(InputAction.CallbackContext context){
        //EquipedItem(int.Parse(idItem.text));
        //string gameobjectFinder = "InventorySlot";
        //GameObject.Find("InventorySlot"+i.ToString())
        /*for(int i = 0; i < 6 ; i++){
            inventorySlots.Add(GameObject.Find("InventorySlot"+i.ToString()));
        }*/

        /*bool resultParse;
        int numberParse;
        resultParse = int.TryParse(idItem.text, out numberParse);
        if(resultParse){
            EquipedItem(numberParse);
        }
        Debug.Log(idItem.text);*/
    }

    public int FindIDMineral(){
        for(int i = 0; i < items.Count; i++){
            if(items[i].obj.name == "Mineral"){
                return i;
            } else{
                return -1;
            }
        }
        return -1;
    }

    public void Consume(int id){
        if(items[id].obj.GetComponent<Item>().item_type == Item.ItemType.Consumables) {
            //Talps
            TaroHealth taroHP = gameObject.GetComponent<TaroHealth>();
            if(items[id].obj.name == "Talps"){
                if(taroHP.health < taroHP.numOfSeeds){
                    taroHP.health++;
                    Debug.Log($"Consumed {items[id].obj.name}");
                    items[id].stack--;
                    items_counters[id].text = items[id].stack.ToString();
                }
            } else if(items[id].obj.name == "Bendicion"){
                taroHP.numOfSeeds++;
                taroHP.health = taroHP.numOfSeeds;
                Debug.Log($"Consumed {items[id].obj.name}");
                items[id].stack--;
                items_counters[id].text = items[id].stack.ToString();
            }else{
                Debug.Log($"Consumed {items[id].obj.name}");
                items[id].stack--;
                items_counters[id].text = items[id].stack.ToString();
            }
            
            if(items[id].stack == 0){
                items[id].obj.SetActive(false);
                equipedItemQty.text = null;
                idItem.text = null;
                equipedItemImg.enabled= false;
                items.Remove(items[id]);
                idSaver = -1;
            } else{        
                EquipedItem(id);
            }

            Update_Ui();
        }
    }

    public void EquipedItem(int id){
        idSaver = id;
        if(items[id].obj.GetComponent<Item>().item_type == Item.ItemType.Consumables) {
            equipedItemQty.text = items[id].stack.ToString();
            idItem.text = id.ToString();
            equipedItemImg.sprite = items_images[id].sprite;
            equipedItemImg.enabled= true;
        }
    }
}

[System.Serializable]
public class InventoryItem{
    public GameObject obj;
    public int stack = 1;
    public InventoryItem(GameObject o, int s = 1){
        obj = o;
        stack = s;
    }
}
