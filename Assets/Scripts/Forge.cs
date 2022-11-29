using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class Forge : MonoBehaviour
{
    public InventorySystem inventoryTaro;
    public TaroAttack taroAttack;

    public ForgeInfo[] weapons;
    public GameObject panelUI;

    public GameObject mineralQtyText;

    private PlayerInputAction playerInputs;
    private bool sashaTrigger;
    private int idMineral;

    public CanvasGroup forgePanel;
    public GameObject forge_GB;

    public CanvasGroup weapon;
    public GameObject infoweapon_GB;

    private Transform needle;
    public Image img_needle;
    public Transform Metal;

    private Vector3 MoveUi = new Vector3(500,1500,0);
    private Vector3 Milim = new Vector3(500,100,0);

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

        forgePanel = GameObject.Find("Forge").GetComponent<CanvasGroup>();
        forge_GB = GameObject.Find("Forge");

        needle = GameObject.Find("NeedleForge").GetComponent<Transform>();
        img_needle = GameObject.Find("NeedleForge").GetComponent<Image>();

        panelUI = GameObject.Find("UIPlayer");

        mineralQtyText = GameObject.Find("QtyForge");

        weapon = GameObject.Find("InfoWeapon").GetComponent<CanvasGroup>();

        infoweapon_GB = GameObject.Find("InfoWeapon");
        
        playerInputs = new PlayerInputAction();

        Metal = GameObject.Find("Metal").GetComponent<Transform>();

        if(PhotonNetwork.IsConnected){
            forge_GB.SetActive(false);
            this.GetComponent<Forge>().enabled = false;
        }

        DOTween.Init();

        forgePanel.DOFade(0f,0f);
        needle.DOScale(0f,0f);
        weapon.DOFade(0f,0f);
        Metal.DOMove(MoveUi,0f);
        forge_GB.SetActive(false);
        infoweapon_GB.SetActive(false);
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
        forge_GB.SetActive(true);
        forgePanel.DOFade(1f, 0.5f).SetDelay(0.25f);
        needle.DOScale(1f,0.5f).SetEase(Ease.OutBounce).SetDelay(0.5f);
        Metal.DOMove(Milim,0.75f);
        panelUI.SetActive(false);
    }

    public void ExitForge()
    {
        needle.DOScale(0f,0.25f);
        forgePanel.DOFade(0f, 0.5f).SetDelay(0.5f).OnComplete(CloseForge);
    }

    public void CloseForge(){
        forge_GB.SetActive(false);
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
        weapon.DOFade(0f,0.25f).OnComplete(() => infoweapon_GB.SetActive(true));
        
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

    public void displayUpgrade(){
        infoweapon_GB.SetActive(true);
        weapon.DOFade(1f,0.5f);
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