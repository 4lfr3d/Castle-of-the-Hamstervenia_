using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    private Vector3 salidaUI = new Vector3(2950,550,0);
    private Vector3 objetoEscalaNormal = new Vector3(1f,1f,1f);
    private Vector3 objetoEscalaReducido = new Vector3(0f,0f,0f);

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

        playerInputs = new PlayerInputAction();
    }

    void Start(){
        // Mandamos el UI fuera de la vista de la camara
        DOTween.Init();

        // Panel de Tienda
        storePanel.GetComponent<Transform>().DOMove(salidaUI, 0f);
        storePanel.GetComponent<Image>().DOFade(0f, 0.5f);

        // Panel de Boton de Talpas
        storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0f);

        // Panel de Boton de Mineral
        storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0f);

        // Panel de Boton de Bendicion
        storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0f);
    }

    void StoreInput(InputAction.CallbackContext context)
    {
        if (talpaTrigger)

        {
            UpdateStore();
            //displayStore();
        }
    }

    public void displayStore()
    {
        // Llamamos la animación de Dotween
        PanelFadeIn();
        panelUI.SetActive(false);
    }

    public void ExitStore()
    {
        // Llamamos la animación de Dotween
        PanelFadeOut();
        panelUI.SetActive(true);
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

    public void PanelFadeIn(){
        Sequence dotweenAnimation = DOTween.Sequence();
        // Panel de Tienda
        dotweenAnimation.Append(storePanel.GetComponent<Image>().DOFade(1, 0.5f));
        dotweenAnimation.Append(storePanel.GetComponent<Transform>().DOMove(entradaUI, 0.5f).SetEase(Ease.OutBounce));

        // Panel de Boton de Talpas
        dotweenAnimation.Join(storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(objetoEscalaNormal, 0.5f).SetEase(Ease.OutBounce));

        // Panel de Boton de Mineral
        dotweenAnimation.Join(storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(objetoEscalaNormal, 0.5f).SetEase(Ease.OutBounce));

        // Panel de Boton de Bendicion
        dotweenAnimation.Join(storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(objetoEscalaNormal, 0.5f).SetEase(Ease.OutBounce));
    }

    public void PanelFadeOut(){
        Sequence dotweenAnimation = DOTween.Sequence();
        
        // Panel de Boton de Talpas
        dotweenAnimation.Join(storePanel.transform.GetChild(3).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0.25f));

        // Panel de Boton de Mineral
        dotweenAnimation.Join(storePanel.transform.GetChild(4).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0.25f));

        // Panel de Boton de Bendicion
        dotweenAnimation.Join(storePanel.transform.GetChild(2).GetComponent<Transform>().DOScale(objetoEscalaReducido, 0.25f));

        // Panel de Tienda
        dotweenAnimation.Join(storePanel.GetComponent<Image>().DOFade(0, 0.25f));
        dotweenAnimation.Join(storePanel.GetComponent<Transform>().DOMove(salidaUI, 0.25f).SetEase(Ease.InOutQuint));
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
            if (info.cost > inventoryTaro.croquetasQty)
            {
                if (info.obj.name == "Talps")
                {
                    talpsText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }

                if (info.obj.name == "Mineral")
                {
                    mineralText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }

                if (info.obj.name == "Bendicion")
                {
                    bendicionText.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }
            }

            if (info.qty == 0)
            {
                if (info.obj.name == "Talps")
                {
                    talpsText.transform.GetChild(1).gameObject.SetActive(true);
                }

                if (info.obj.name == "Mineral")
                {
                    mineralText.transform.GetChild(1).gameObject.SetActive(true);
                }

                if (info.obj.name == "Bendicion")
                {
                    bendicionText.transform.GetChild(1).gameObject.SetActive(true);
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
