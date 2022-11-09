using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
        playerInputs = new PlayerInputAction();
        storePanel.SetActive(false);
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
        storePanel.SetActive(true);
        panelUI.SetActive(false);
    }

    public void ExitStore()
    {
        storePanel.SetActive(false);
        panelUI.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Talpa")
        {
            talpaTrigger = true;
            interactionIcon.gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 75, 0);
            interactionIcon.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        } else
        {
            talpaTrigger = false;
            interactionIcon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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

    void UpdateStore()
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
