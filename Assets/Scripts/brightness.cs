using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class brightness : MonoBehaviour
{

    public Slider brightnessSlider;

    public PostProcessProfile brightNess;
    public PostProcessLayer layer;

    AutoExposure exposure;
    // Start is called before the first frame update
    void Start()
    {
        brightNess.TryGetSettings(out exposure);
        AdjustBrightness(brightnessSlider.value);
    }

    public void AdjustBrightness(float value){
        if(value != 0 ){
            exposure.keyValue.value = value;
        }
        else{
            exposure.keyValue.value = .05f;
        }
    }
}
