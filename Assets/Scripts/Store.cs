using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Store : MonoBehaviour
{
    public InventorySystem inventoryTaro;
    public StoreInfo[] items;

    public GameObject talpsText;
    public GameObject mineralText;
    public GameObject bendicionText;

    public TMP_Text croquetas;

    void Start()
    {
        croquetas.text =  "x " + inventoryTaro.croquetasQty.ToString();

        bendicionText.transform.GetChild(1).gameObject.SetActive(true);

        talpsText.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = items[0].obj.name;
        talpsText.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = items[0].obj.GetComponent<Item>().item_desc;
        talpsText.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TMP_Text>().text = items[0].cost.ToString();
        talpsText.transform.GetChild(0).GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Unlimited";
    }

    void Update()
    {

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
