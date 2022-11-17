using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoaderController : MonoBehaviour
{
    public TextMeshProUGUI loaderText;
    public float delay = 4f;
    public int maxDots = 4;
    
    private int count = 0;

    void Start()
    {
        StartCoroutine(GameLoaderScene());
        StartCoroutine(LoadingText());
    }

    IEnumerator LoadingText(){
        yield return new WaitForSeconds(0.25f);
        loaderText.text = "Cargando ";
        for(int i = 0; i < count; i++){
            loaderText.text = loaderText + ".";
        }
        count++;
        if(count == maxDots){
            count = 0;
        }
        StartCoroutine(LoadingText());
    }

    IEnumerator GameLoaderScene(){
        AsyncOperation op;
        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Nueva Escena");

        op.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        op.allowSceneActivation = true;
    }
}
