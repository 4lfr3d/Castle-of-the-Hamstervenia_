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
    private float moveTime = 2f;

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

        dialogueBox_transform = GameObject.Find("DialogBox").GetComponent<Transform>();

        dialogueBox_Bg = GameObject.Find("DIalogBG").GetComponent<Image>();

        DOTween.Init();

        dialogueBox_transform.DOMove(salidaDialogo,0);
        dialogueBox_Bg.DOFade(0f, fadeTime);
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
        entradaPanelSecuencia.Join(dialogueBox_Bg.DOFade(1, fadeTime).SetDelay(1f));
        entradaPanelSecuencia.Join(dialogueBox_transform.DOMove(entradaDialogo,moveTime).SetEase(Ease.OutBounce));
    }

    public void PanelFadeOut(){
        Sequence salidaPanelSecuencia = DOTween.Sequence();
        salidaPanelSecuencia.Join(dialogueBox_Bg.DOFade(0, fadeTime).SetDelay(0.5f));
        salidaPanelSecuencia.Join(dialogueBox_transform.DOMove(salidaDialogo,moveTime).SetEase(Ease.InOutQuint));
    }
}
