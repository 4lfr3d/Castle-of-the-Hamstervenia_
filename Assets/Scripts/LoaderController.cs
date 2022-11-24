using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoaderController : MonoBehaviour
{
    public TextMeshProUGUI loaderText;
    public float delay = 10f;
    public int maxDots = 3;

    void Start()
    {
        StartCoroutine(LoadingText());
        StartCoroutine(GameLoaderScene());
    }

    IEnumerator LoadingText(){
        loaderText.text = "Cargando ";
        for(int i = 0; i <= maxDots; i++){
            yield return new WaitForSeconds(0.25f);
            loaderText.text = loaderText.text + ".";
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
