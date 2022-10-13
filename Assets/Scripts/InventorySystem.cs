using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem{
        public GameObject obj;
        public int stack = 1;

        public InventoryItem(GameObject o, int s = 1){
            obj = o;
            stack = s;
        }
    }

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

    public void PickUp(GameObject item){

        if(item.GetComponent<Item>().stackable){
            //check if existing item, yes stack, no add
            InventoryItem exisitingItem = items.Find(x => x.obj.name == item.name);

            if(exisitingItem != null){
                exisitingItem.stack++;
                item.GetComponent<Item>().count++;
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

        Update_Ui();
    }

    public bool CanPickUp(){
        if(items.Count >= items_images.Length){
            return false;
        } else{
            return true;
        }
    }


    void Update_Ui(){

        HideAll();
        
        for (int i = 0; i < items.Count; i++)
        {
            items_images[i].sprite = items[i].obj.GetComponent<SpriteRenderer>().sprite;
            //items_counters[i].text = items[i].obj.GetComponent<Item>().count.ToString();
            if(items[i].stack == 1){
                items_counters[i].text = " ";
            }
            else{
                items_counters[i].text = items[i].stack.ToString();
            }
            items_counters[i].gameObject.SetActive(true);
            items_images[i].gameObject.SetActive(true);
        }
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
            //counter.text = " ";
        }
        else
        {
            //counter.text = items[id].stack.ToString();
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

    void Update(){
        if(Input.GetKeyDown("f")){
            Consume(int.Parse(idItem.text));
        }
    }

    public void Consume(int id){
        if(items[id].obj.GetComponent<Item>().item_type == Item.ItemType.Consumables) {
            //Talps
            TaroHealth taroHP = gameObject.GetComponent<TaroHealth>();
            if(items[id].obj.name == "Talps"){
                if(taroHP.health < taroHP.numOfSeeds){
                    taroHP.health++;
                    Debug.Log(taroHP.health);
                    Debug.Log($"Consumed {items[id].obj.name}");
                    
                    items[id].stack--;
                    items_counters[id].text = items[id].stack.ToString();
                }
            } else{
                Debug.Log($"Consumed {items[id].obj.name}");
                items[id].stack--;
                items_counters[id].text = items[id].stack.ToString();
            }
            
            if(items[id].stack == 0){
                Destroy(items[id].obj, 0.1f);
                equipedItemQty.text = null;
                idItem.text = null;
            equipedItemImg.enabled= false;
                items.Remove(items[id]);
            } else{        
                EquipedItem(id);
            }

            Update_Ui();
        }
    }

    public void EquipedItem(int id){
        if(items[id].obj.GetComponent<Item>().item_type == Item.ItemType.Consumables) {
            equipedItemQty.text = items[id].stack.ToString();
            idItem.text = id.ToString();
            equipedItemImg.sprite = items_images[id].sprite;
            equipedItemImg.enabled= true;
        }
    }
    

}
