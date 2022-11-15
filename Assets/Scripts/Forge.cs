using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Forge : MonoBehaviour
{
    public InventorySystem inventoryTaro;
    public TaroAttack taroAttack;

    public ForgeInfo[] weapons;
    public GameObject forgePanel;
    public GameObject panelUI;

    public GameObject mineralQtyText;

    private PlayerInputAction playerInputs;
    private bool sashaTrigger;
    private int idMineral;

    private void OnEnable()
    {
        playerInputs.Player.Interaction.performed += ForgeInput;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Interaction.Disable();
    }

    private void Awake()
    {
        weapons[0].button = GameObject.Find("NeedleForge");

        forgePanel = GameObject.Find("Forge");
        panelUI = GameObject.Find("UIPlayer");
        mineralQtyText = GameObject.Find("QtyForge");
        
        playerInputs = new PlayerInputAction();
        forgePanel.SetActive(false);
    }


    void Update(){
    }

    void ForgeInput(InputAction.CallbackContext context)
    {
        if (sashaTrigger)
        {
            UpdateForge();
            //displayForge();
        }
    }

    public void displayForge()
    {
        forgePanel.SetActive(true);
        panelUI.SetActive(false);
    }

    public void ExitForge()
    {
        forgePanel.SetActive(false);
        panelUI.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Sasha")
        {
            sashaTrigger = true;
        } else
        {
            sashaTrigger = false;
        }
    }

    public void Mejora(int id)
    {
        for(int i = 0; i < weapons[id].cost; i++){
            inventoryTaro.Consume(inventoryTaro.FindIDMineral());
        }
        weapons[id].actualLevel++;
        taroAttack.damage = weapons[id].actualLevel;
        UpdateForge();
        weapons[id].cost = weapons[id].cost + 2;
        weapons[id].button.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void UpdateForge(){
        idMineral = inventoryTaro.FindIDMineral();
            if(idMineral != -1){
                if(inventoryTaro.items[idMineral].stack >= weapons[0].cost && weapons[0].actualLevel < weapons[0].maxLevel){
                    weapons[0].button.GetComponent<Button>().interactable = true;
                    weapons[0].button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = true;
                    mineralQtyText.GetComponent<TMP_Text>().text = inventoryTaro.items[idMineral].stack.ToString();
                    weapons[0].button.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Level " + (weapons[0].actualLevel + 1).ToString();
                    weapons[0].button.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Level " + (weapons[0].actualLevel).ToString();
                    weapons[0].button.transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Level " + (weapons[0].actualLevel + 1).ToString();
                    weapons[0].button.transform.GetChild(5).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Damage " + (weapons[0].actualLevel + 1).ToString();
                } else{
                    weapons[0].button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
                    weapons[0].button.GetComponent<Button>().interactable = false;
                }

                if(weapons[0].actualLevel == weapons[0].maxLevel){
                    weapons[0].button.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Level Max";
                    weapons[0].button.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Level Max";
                }
            }
            else{
                weapons[0].button.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Level " + (weapons[0].actualLevel + 1).ToString();
                weapons[0].button.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Level " + (weapons[0].actualLevel).ToString();
                mineralQtyText.GetComponent<TMP_Text>().text = "0";
                weapons[0].button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
                weapons[0].button.GetComponent<Button>().interactable = false;
            }
    }

    public void displayUpgrade(GameObject weapon){
        weapon.SetActive(true);
    }
}

[System.Serializable]
public class ForgeInfo
{
    public string name;
    public int cost;
    public int actualLevel;
    public int maxLevel;
    public GameObject button;

    public ForgeInfo(string n, int c, int al, int ml, GameObject b)
    {
        name = n;
        cost = c;
        actualLevel = al;
        maxLevel = ml;
        button = b;

    }
}