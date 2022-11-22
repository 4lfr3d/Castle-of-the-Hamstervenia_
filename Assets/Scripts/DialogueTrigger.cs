using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class DialogueTrigger : MonoBehaviour
{
    
    //change to when player gets close to npc

    public List<Dialogue> dialogue;

    public Store store;

    public Forge forge;

    public GameObject conversationIcon;

    private PlayerInputAction playerInputs;

    public GameObject dialogoptions_GB;
    public CanvasGroup dialogueOptions_Panel;


    private Transform tienda_boton;
    private Transform platica_boton;
    private Transform salida_boton;

    private Vector3 objetoScalaNormal = new Vector3(1.3359f,1.403096f,1.3359f);
    private Vector3 objetoScalaReducido = new Vector3(0f,0f,0f);


    public List<GameObject> options = new List<GameObject>();

    private float scaleTime = 0.25f;

    private Dialogue selector;

    private bool chat;

    private string nombreVendedor;
    private string tagNpc;

    private float fadeTime = 0.5f;

    private void Awake(){
        conversationIcon = GameObject.Find("ConversationTrigger");
        dialogueOptions_Panel = GameObject.Find("DialogOptions").GetComponent<CanvasGroup>();

        dialogoptions_GB = GameObject.Find("DialogOptions");

        tienda_boton = GameObject.Find("Tienda").GetComponent<Transform>();
        platica_boton = GameObject.Find("Platicar").GetComponent<Transform>();
        salida_boton = GameObject.Find("DejarChat").GetComponent<Transform>();

        playerInputs = new PlayerInputAction();
        chat = false;

        DOTween.Init();

        dialogueOptions_Panel.DOFade(0f,0f);

        ScaleDownOptions();
    }

    private void OnEnable(){
        playerInputs.Player.Interaction.performed += TriggerDialogue;
        playerInputs.Player.Interaction.Enable();
    }

    private void OnDisable(){
        playerInputs.Player.Interaction.Disable();
    }

    public void showtienda(){
        Debug.Log("Display store");
        if(nombreVendedor == "Talpa") store.displayStore();
        if(nombreVendedor == "Sasha") forge.displayForge();
    }

    public void showDialogo(){
        Debug.Log("Display dialogue");
        DialogoSelector();
        DialogueManager.instance.StartDialogue(selector);
    }

    public void DialogoSelector(){
        foreach (Dialogue dialogo in dialogue)
        {
            if(nombreVendedor == dialogo.name){
                selector = dialogo;
            }
        }
    }

    public void TriggerDialogue(InputAction.CallbackContext context){

        DialogoSelector();
        if(chat){
            if(tagNpc == "NPCVendor") {
                dialogoptions_GB.SetActive(true);
                dialogueOptions_Panel.DOFade(1f,fadeTime).OnComplete(ScaleUpOptions);
            }
            else DialogueManager.instance.StartDialogue(selector);
        }        
    }

    public void ScaleUpOptions(){
        Sequence escalarArriba = DOTween.Sequence();
        escalarArriba.Append(tienda_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
        escalarArriba.Append(platica_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
        escalarArriba.Append(salida_boton.DOScale(objetoScalaNormal, scaleTime).SetEase(Ease.OutBounce));
    }

    public void ScaleDownOptions(){
        Sequence escalarAbajo = DOTween.Sequence();
        escalarAbajo.Join(tienda_boton.DOScale(objetoScalaReducido, 0f));
        escalarAbajo.Join(platica_boton.DOScale(objetoScalaReducido, 0f));
        escalarAbajo.Join(salida_boton.DOScale(objetoScalaReducido, 0f).OnComplete(()=>Activate_Panel(false)));

    }

    public void OpcionElegida(){
        Sequence opcionelegida = DOTween.Sequence();
        opcionelegida.Join(dialogueOptions_Panel.DOFade(0,0.5f));
        opcionelegida.Append(tienda_boton.DOScale(objetoScalaReducido, 0f).SetDelay(1f));
        opcionelegida.Append(platica_boton.DOScale(objetoScalaReducido, 0f));
        opcionelegida.Append(salida_boton.DOScale(objetoScalaReducido, 0f).OnComplete(()=>Activate_Panel(false)));

    }

    public void Activate_Panel(bool activate){
        dialogoptions_GB.SetActive(activate);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "NPCVendor" || other.tag == "NPCs")
        {
            nombreVendedor = other.gameObject.name;
            tagNpc = other.tag;
            chat = true;
            conversationIcon.gameObject.transform.position = other.gameObject.transform.position + new Vector3(-20, 75, 0);
            conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } 
        else
        {
            chat = false;
           conversationIcon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
