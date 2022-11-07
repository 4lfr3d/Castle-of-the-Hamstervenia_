using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinsManager : MonoBehaviour
{
    public InventorySystem inventoryTaro;
    public int coinsToAdd;

    void Update()
    {
        this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = inventoryTaro.croquetasQty.ToString();
    }

    public void addCoins(){
        StartCoroutine(ShowCoinsAdd());
    }

    IEnumerator ShowCoinsAdd () {
        this.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "+ " + coinsToAdd;
        yield return new WaitForSeconds(3);
        this.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = " ";
        coinsToAdd = 0;
    }
}
