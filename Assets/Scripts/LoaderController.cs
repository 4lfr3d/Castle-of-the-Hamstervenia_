using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LoaderController : MonoBehaviour
{
    public TextMeshProUGUI loaderText;
    public TextMeshProUGUI hintText;
    public Image itemImage;
    public Sprite[] items;
    public float delay = 10f;
    public int maxDots = 3;
    public string nextScene;

    private string[] hints = {"El gancho de Taro no funciona en vidrio", 
                              "Talpa tiene items interesantes en su tienda",
                              "Sasha te ayudara a mejorar tu aguja",
                              "Las ratas tienen mayor vida que las cucarachas",
                              "Las bendiciones te ayudaran a aumentar el máximo de vidas",
                              "Consume Talpas para curarte"};

    void Start()
    {
        DOTween.Init();
        StartCoroutine(ItemsImageMove());
        StartCoroutine(LoadingText());
        StartCoroutine(GameLoaderScene());
        RandomHint();
    }

    void RandomHint(){
        hintText.text = hints[Random.Range(0, hints.Length)];
    }

    IEnumerator ItemsImageMove(){
        Sequence dotweenAnimation = DOTween.Sequence();

        dotweenAnimation.Append(itemImage.GetComponent<Transform>().DOMove(new Vector3(750,250,0), 0f));
        itemImage.GetComponent<Image>().sprite = items[Random.Range(0, items.Length)];
        dotweenAnimation.Append(itemImage.GetComponent<Transform>().DOMove(new Vector3(1500,250,0), 2f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(ItemsImageMove());
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
        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        yield return new WaitForSeconds(delay);

        op.allowSceneActivation = true;
    }
}
