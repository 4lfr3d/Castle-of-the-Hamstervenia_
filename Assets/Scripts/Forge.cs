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
        playerInputs = new PlayerInputAction();
        forgePanel.SetActive(false);
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
        inventoryTaro.items[idMineral].stack = inventoryTaro.items[idMineral].stack - weapons[id].cost;
        weapons[id].actualLevel++;
        taroAttack.damage = weapons[id].actualLevel;
        weapons[id].cost = weapons[id].cost + 2;
        UpdateForge();
        weapons[id].button.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void UpdateForge(){
        idMineral = inventoryTaro.FindIDMineral();
        foreach(ForgeInfo forgeitem in weapons){
            if(idMineral != -1){
                if(inventoryTaro.items[idMineral].stack >= forgeitem.cost && forgeitem.actualLevel < forgeitem.maxLevel){
                    forgeitem.button.GetComponent<Button>().interactable = true;
                    forgeitem.button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = true;
                    mineralQtyText.GetComponent<TMP_Text>().text = inventoryTaro.items[idMineral].stack.ToString();
                    forgeitem.button.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Level " + (forgeitem.actualLevel + 1).ToString();
                    forgeitem.button.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Level " + (forgeitem.actualLevel).ToString();
                    forgeitem.button.transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Level " + (forgeitem.actualLevel + 1).ToString();
                    forgeitem.button.transform.GetChild(5).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Damage " + (forgeitem.actualLevel + 1).ToString();
                } else{
                    forgeitem.button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
                    forgeitem.button.GetComponent<Button>().interactable = false;
                }

                if(forgeitem.actualLevel == forgeitem.maxLevel){
                    forgeitem.button.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Level Max";
                    forgeitem.button.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = "Level Max";
                }
            }
            else{
                mineralQtyText.GetComponent<TMP_Text>().text = "0";
                forgeitem.button.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
                forgeitem.button.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void displayUpgrade(GameObject weapon){
        weapon.SetActive(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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