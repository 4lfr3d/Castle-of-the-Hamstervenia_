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

    //public Animator animator;

    public Transform dialogueBox;

    private Vector3 entradaDialogo = new Vector3(1000,300,0);
    private Vector3 salidaDialogo = new Vector3(3000,300,0);

    private Queue<string> sentences;

    public static DialogueManager instance;


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

        dialogueBox = GameObject.Find("DialogBox").GetComponent<Transform>();

        DOTween.Init();

        dialogueBox.DOMove(salidaDialogo,0);
    }

    public void StartDialogue(Dialogue dialogue){

      // animator.SetBool("IsOpen",true);

        dialogueBox.DOMove(entradaDialogo ,1);

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
        //animator.SetBool("IsOpen", false);
        dialogueBox.DOMove(salidaDialogo,2);
        Debug.Log("End of conversation");
    }
}
