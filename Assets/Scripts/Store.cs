using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class Store : MonoBehaviour
{
    public InventorySystem inventoryTaro;
    public StoreInfo[] items;

    public GameObject talpsText;
    public GameObject mineralText;
    public GameObject bendicionText;
    public TMP_Text croquetas;
    public GameObject storePanel;
    public GameObject panelUI;

    public GameObject interactionIcon;

    private bool talpaTrigger;
    private PlayerInputAction playerInputs;

    // Vectores para el movimiento del UI en DoTween
    private Vector3 entradaUI = new Vector3(950,550,0);
    private Vector3 salidaUI = new Vector3(2850,550,0);

    private void OnEnable()
    {
        playerInputs.Player.Interaction.performed += StoreInput;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Interaction.Disable();
    }

    private void Awake()
    {
        GameObject listItems = GameObject.Find("StoreItems");
        items[0].obj = listItems.transform.GetChild(0).gameObject;
        items[1].obj = listItems.transform.GetChild(1).gameObject;
        items[2].obj = listItems.transform.GetChild(2).gameObject;

        talpsText = GameObject.Find("TalpsButton");
        mineralText = GameObject.Find("MineralButton");
        bendicionText = GameObject.Find("BendicionButton");
        croquetas = GameObject.Find("CroquetasText").GetComponent<TMP_Text>();
        storePanel = GameObject.Find("Store");
        panelUI = GameObject.Find("UIPlayer");
        //interactionIcon = GameObject.Find("SaleIcon");

        if(PhotonNetwork.IsConnected){
            storePanel.SetActive(false);
            this.GetComponent<Store>().enabled = false;
        }

        playerInputs = new PlayerInputAction();
    }

    void Start(){
        // Inicializamos DoTween en el Script
        DOTween.Init();

        // Panel de Tienda
        storePanel.GetComponent<Transform>().DOMove(salidaUI, 0f);
        storePanel.GetComponent<Image>().DOFade(0f, 0.5f);

        // Panel de Boton de Talpas
        storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(0f, 0f);

        // Panel de Boton de Mineral
        storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(0f, 0f);

        // Panel de Boton de Bendicion
        storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(0f, 0f).OnComplete(() => storePanel.SetActive(false));
    }

    void StoreInput(InputAction.CallbackContext context)
    {
        
    }

    public void displayStore()
    {
        UpdateStore();

        storePanel.SetActive(true);
        // Llamamos la animación de Dotween de Entrada
        InAnimation();
        panelUI.SetActive(false);
    }

    public void ExitStore()
    {
        // Llamamos la animación de Dotween de Salida
        OutAnimation();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Talpa")
        {
            talpaTrigger = true;

        } else
        {
            talpaTrigger = false;
        }
    }

    public void Trueque(int id)
    {
        if (items[id].cost <= inventoryTaro.croquetasQty)
        {
            inventoryTaro.croquetasQty = inventoryTaro.croquetasQty - items[id].cost;
            inventoryTaro.PickUp(items[id].obj);
            croquetas.text = "x " + inventoryTaro.croquetasQty.ToString();
            items[id].qty--;
            UpdateStore();
        }
    }

    // Animación de Dotween de Entrada
    public void InAnimation(){
        Sequence dotweenAnimation = DOTween.Sequence();
        
        // Panel de Tienda
        dotweenAnimation.Append(storePanel.GetComponent<Image>().DOFade(1, 0.75f));
        dotweenAnimation.Append(storePanel.GetComponent<Transform>().DOMove(entradaUI, 0.75f).SetEase(Ease.OutBounce));

        // Panel de Boton de Talpas
        dotweenAnimation.Append(storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(1f, 0.25f).SetEase(Ease.OutBounce));

        // Panel de Boton de Mineral
        dotweenAnimation.Append(storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(1f, 0.25f).SetEase(Ease.OutBounce));

        // Panel de Boton de Bendicion
        dotweenAnimation.Append(storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(1f, 0.25f).SetEase(Ease.OutBounce));
    }

    // Animación de DoTween de Salida
    public void OutAnimation(){
        Sequence dotweenAnimation = DOTween.Sequence();
        
        // Panel de Boton de Talpas
        dotweenAnimation.Append(storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(0f, 0.2f));

        // Panel de Boton de Mineral
        dotweenAnimation.Append(storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(0f, 0.2f));

        // Panel de Boton de Bendicion
        dotweenAnimation.Append(storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(0f, 0.2f));

        // Panel de Tienda
        dotweenAnimation.Join(storePanel.GetComponent<Image>().DOFade(0, 0.4f));
        dotweenAnimation.Join(storePanel.GetComponent<Transform>().DOMove(salidaUI, 0.4f).SetEase(Ease.InOutQuint).OnComplete(() => panelUI.SetActive(true)));
    }

    public void UpdateStore()
    {
        croquetas.text =  "x " + inventoryTaro.croquetasQty.ToString();
        
        talpsText.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = items[0].obj.name;
        talpsText.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = items[0].obj.GetComponent<Item>().item_desc;
        talpsText.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TMP_Text>().text = items[0].cost.ToString();
        talpsText.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Unlimited";

        mineralText.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = items[1].obj.name;
        mineralText.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = items[1].obj.GetComponent<Item>().item_desc;
        mineralText.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TMP_Text>().text = items[1].cost.ToString();
        mineralText.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_Text>().text = "x" + items[1].qty.ToString();

        bendicionText.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = items[2].obj.name;
        bendicionText.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = items[2].obj.GetComponent<Item>().item_desc;
        bendicionText.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TMP_Text>().text = items[2].cost.ToString();
        bendicionText.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_Text>().text = "x" + items[2].qty.ToString();

        foreach (StoreInfo info in items)
        {
            if (info.qty == 0){
                if (info.obj.name == "Talps")
                {
                    talpsText.transform.GetChild(1).gameObject.SetActive(true);
                }

                else if (info.obj.name == "Mineral")
                {
                    mineralText.transform.GetChild(1).gameObject.SetActive(true);
                }

                else if (info.obj.name == "Bendicion")
                {
                    bendicionText.transform.GetChild(1).gameObject.SetActive(true);
                }
            }

            if (info.cost > inventoryTaro.croquetasQty){
                if (info.obj.name == "Talps")
                {
                    talpsText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }

                else if (info.obj.name == "Mineral")
                {
                    mineralText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }

                else if (info.obj.name == "Bendicion")
                {
                    bendicionText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }
            }
            else {
                if (info.obj.name == "Talps")
                {
                    talpsText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                }

                else if (info.obj.name == "Mineral")
                {
                    mineralText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                }

                else if (info.obj.name == "Bendicion")
                {
                    bendicionText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
    }
}

[System.Serializable]
public class StoreInfo
{
    public GameObject obj;
    public int qty;
    public int cost;

    public StoreInfo(GameObject o, int q, int c)
    {
        obj = o;
        qty = q;
        cost = c;
    }
}
