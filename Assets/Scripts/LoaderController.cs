using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoaderController : MonoBehaviour
{
    public TextMeshProUGUI loaderText;
    public float delay = 10f;
    public int maxDots = 5;
    
    private int count = 0;

    void Start()
    {
        StartCoroutine(GameLoaderScene());
        StartCoroutine(LoadingText());
    }

    IEnumerator LoadingText(){
        loaderText.text = "Cargando ";

        // yield return new WaitForSeconds(0.25f);

        for(int i = 0; i < maxDots; i++){
            loaderText.text = loaderText.text + ".";
        }
        count++;
        if(count == maxDots){
            count = 0;
        }
        StartCoroutine(LoadingText());
    }

    IEnumerator GameLoaderScene(){
        AsyncOperation op;
        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("firstScene");

        op.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        op.allowSceneActivation = true;
    }
}
