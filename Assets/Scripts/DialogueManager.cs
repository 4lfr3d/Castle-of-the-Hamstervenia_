using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text dialogueTxt;

    public Animator animator;

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
    }

    public void StartDialogue(Dialogue dialogue){

       animator.SetBool("IsOpen",true);

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
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation");
    }
}
