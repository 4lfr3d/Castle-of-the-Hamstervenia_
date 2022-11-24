using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text dialogueTxt;

    private Queue<string> sentences;
    public static DialogueManager instance;


    [Header("Dotween DialogueBox")]
    public Transform dialogueBox_transform;
    public Image dialogueBox_Bg;
    
    private float fadeTime = 1f;
    private float duration = 1f;
    private float moveTime = 1.5f;

    private Vector3 entradaDialogo = new Vector3(1000,300,0);
    private Vector3 salidaDialogo = new Vector3(3000,300,0);



    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sentences = new Queue<string>();

        DOTween.Init();


        /* Está configurando el estado inicial del cuadro de diálogo. Moviendo el cuadro del dialogo fuera del canvas y desvaneciendo su opacidado a 0 en 1 segundo */
        dialogueBox_transform.DOMove(salidaDialogo,0);
        dialogueBox_Bg.DOFade(0f, fadeTime);
    }

    void Update(){
        if(dialogueTxt == null){
            dialogueTxt = GameObject.Find("DialogText").GetComponent<TMP_Text>();
        }
        if(nameTxt == null){
            nameTxt = GameObject.Find("NPCname").GetComponent<TMP_Text>();
        }
        if(dialogueBox_transform == null){
            dialogueBox_transform = GameObject.Find("DialogBox").GetComponent<Transform>();
            dialogueBox_transform.DOMove(salidaDialogo,0);
        }
        if(dialogueBox_Bg == null){
            dialogueBox_Bg = GameObject.Find("DIalogBG").GetComponent<Image>();
            dialogueBox_Bg.DOFade(0f, fadeTime);
        }
    
    }

    public void StartDialogue(Dialogue dialogue){

        PanelFadeIn();

        nameTxt.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            Debug.Log("enqueue sentence");
            sentences.Enqueue(sentence);   
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        Debug.Log("displaysentence");
        
        if(sentences.Count == 0){
            Debug.Log("end dialogue");
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);

        //si ya esta corriendo un coroutine, lo detiene y corre una nueva
        StopAllCoroutines();
        Debug.Log("stop coroutines");

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence){
        Debug.Log("type sentence");

        dialogueTxt.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTxt.text += letter;
            yield return null;
        }
    }

    void EndDialogue(){
        PanelFadeOut();
        dialogueTxt.text = "";
        nameTxt.text = "";
        Debug.Log("End of conversation");
    }

    public void PanelFadeIn(){
        Sequence entradaPanelSecuencia = DOTween.Sequence();
        /* Moviendo el dialogBox_transform a la posición de entradaDialogo en
        moveTime segundos y usando la aceleración Ease.InOutBack. */
        entradaPanelSecuencia.Join(dialogueBox_transform.DOMove(entradaDialogo,moveTime).SetEase(Ease.InOutBack));
        /* Desvaneciendo el dialogBox_Bg a 1 en 1 segundo y retrasándolo por
        1 segundo. */
        entradaPanelSecuencia.Join(dialogueBox_Bg.DOFade(1, fadeTime).SetDelay(1f));
    }

    public void PanelFadeOut(){
        Sequence salidaPanelSecuencia = DOTween.Sequence();
        /* Desvaneciendo el dialogBox_Bg a 0 en fadeTime segundos y
        retrasándolo por 0.5 segundos. */
        salidaPanelSecuencia.Join(dialogueBox_Bg.DOFade(0, fadeTime).SetDelay(0.5f));
        /* Moviendo el dialogBox_transform a la posición de salidaDialogo en
        moveTime segundos y usando la aceleración Ease.InOutQuint. */
        salidaPanelSecuencia.Join(dialogueBox_transform.DOMove(salidaDialogo,moveTime).SetEase(Ease.InOutQuint));
    }
}
