using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LoaderController : MonoBehaviour
{
    public TextMeshProUGUI loaderText;
    public TextMeshProUGUI hintText;
    public int maxDots = 3;

    private string[] hints = {"El gancho de Taro no funciona en vidrio", 
                              "Talpa tiene items interesantes en su tienda",
                              "Sasha te ayudara a mejorar tu aguja",
                              "Las ratas tienen mayor vida que las cucarachas",
                              "Las bendiciones te ayudaran a aumentar el máximo de vidas",
                              "Consume Talpas para curarte"};

    void Start()
    {
        StartCoroutine(LoadingText());
        StartCoroutine(GameLoaderScene());
        RandomHint();
    }

    void RandomHint(){
        hintText.text = hints[Random.Range(0, hints.Length)];
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
        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(PlayerPrefs.GetString("LoadScreenNextScene"));

        op.allowSceneActivation = false;

        yield return new WaitForSeconds(PlayerPrefs.GetFloat("LoadScreenDelay"));

        op.allowSceneActivation = true;
    }
}
