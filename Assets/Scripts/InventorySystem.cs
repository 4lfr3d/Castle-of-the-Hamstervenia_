using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

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
            items_images[i].color = new Color(items_images[i].color.r, items_images[i].color.g, items_images[i].color.b, 0f);
            items_counters[i] = GameObject.Find("InventorySlot" + i.ToString()).transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
            items_counters[i].text = "";
        }

        description_image = GameObject.Find("DescriptionImage").GetComponent<Image>();
        hud_item = GameObject.Find("ConsumableItem").GetComponent<Image>();
        description_Title = GameObject.Find("Infotitle").GetComponent<TMP_Text>();
        item_description = GameObject.Find("ItemDescription").GetComponent<TMP_Text>();
        equipedItemImg = GameObject.Find("ConsumableItem").GetComponent<Image>();
        equipedItemQty = GameObject.Find("ConsumableText").GetComponent<TMP_Text>();
        idItem = GameObject.Find("idHolderInvisible").GetComponent<TMP_Text>();
        croquetasQtyTxt = GameObject.Find("CroquetasQty").GetComponent<TMP_Text>();
        cm = GameObject.Find("Coins").GetComponent<CoinsManager>();

        item_description.text = "";
        description_Title.text = "";
        
        description_image.DOFade(0f,0f);

        playerInputs = new PlayerInputAction();
    }

    void Start(){
        //Inicia DOTWeen
        DOTween.Init();
    }

    private void OnEnable(){
        playerInputs.Player.CONSUMETHECHILD.performed += DoConsume;
        playerInputs.Player.CONSUMETHECHILD.Enable();
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
            items_images[i].color = new Color(items_images[i].color.r, items_images[i].color.g, items_images[i].color.b, 255f);
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

        HideDescriptionALL();
    }

    //mostrar info al hacer hover en inventario

    public void ShowDescription(int id){
        description_image.sprite = items_images[id].sprite;

        Vector3 valorFinal = new Vector3(1.5f,1.5f,1.5f);


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

        //Hace la animación de Scale en el objeto en el que se hace hover, después de completar se hace la función OnComplete
        items_images[id].transform.DOScale(valorFinal, 1).OnComplete(FadeItems);
    }

    //Función para hacer el OnComplete en DOTween
    public void FadeItems(){
        //Secuencia de Dotween de nombre FadeItems
        Sequence fadeThings = DOTween.Sequence();
        //Animaciones que se añaden a la secuencia (se hacen en ese orden)
        fadeThings.Append(description_image.DOFade(2, 1).SetEase(Ease.OutBounce));
        fadeThings.Append(description_Title.DOFade(2, 1).SetEase(Ease.OutBounce));
        fadeThings.Append(item_description.DOFade(2, 1).SetEase(Ease.OutBounce));
    }

    //ocultar informacion descriptcion
    public void HideDescription(int id){
        Vector3 valorFinal = new Vector3(1,1,1);
        items_images[id].transform.DOScale(valorFinal, .5f);

        //Delay de DOTween para cuando se quita el hover
        description_image.DOFade(0, 1).SetDelay(.25f);
        description_Title.DOFade(0, 1).SetDelay(.25f);
        item_description.DOFade(0, 1).SetDelay(.25f);
    }

    public void HideDescriptionALL(){
        //Se hace el Fade para estas imagénes (es un método necesario para que no despliegue la información)
        description_image.DOFade(0, .5f);
        description_Title.DOFade(0, .5f);
        item_description.DOFade(0, .5f);
    }

    public void DoConsume(InputAction.CallbackContext context){
        bool resultParse;
        int numberParse;
        resultParse = int.TryParse(idItem.text, out numberParse);
        if(resultParse){
            Consume(numberParse);
        }
    }

    public int FindIDMineral(){
        for(int i = 0; i < items.Count; i++){
            if(items[i].obj.name == "Mineral"){
                return i;
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
        if(id >= 0 && id < items.Count){
            if(items[id].obj.GetComponent<Item>().item_type == Item.ItemType.Consumables) {
                equipedItemQty.text = items[id].stack.ToString();
                idItem.text = id.ToString();
                equipedItemImg.sprite = items_images[id].sprite;
                equipedItemImg.enabled= true;
            }
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
