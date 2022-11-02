using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    #region SFX
    
    [Header("SFXs")]
    [SerializeField] public AudioSource jump;
    [SerializeField] public AudioClip jumpsound;
 

    [SerializeField] public AudioSource move;
    [SerializeField] public AudioClip movesound;


    [SerializeField] public AudioSource waterdrop;
    [SerializeField] public AudioClip waterdropsound;


    [SerializeField] public AudioSource gancho;
    [SerializeField] public AudioClip ganchoSound;


    [SerializeField] public AudioSource ratonAtaque;
    [SerializeField] public AudioClip ratonAttkSonido;


    [SerializeField] public AudioSource taroAtaque;
    [SerializeField] public AudioClip taroAtaqueSound;


    [SerializeField] public AudioSource taroDash;
    [SerializeField] public AudioClip taroDashSonido;


    #endregion

    #region MUSIC

    [Header("Music")]

    [SerializeField] List<AudioClip> musicZones = new List<AudioClip>();


    public Transform detectionPoint;
    public LayerMask detectionLayer;
    public const float detectionRadius = 5f;
    public GameObject detectedObject;

    private bool detectMusic = false;
    private AudioSource music01;
    private int track;

    private AudioClip prevClip;
    private AudioClip currClip;

    #endregion

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        } 
    }

    //Primera version, mejorar codigo en siguentes versiones

    #region SFX_FUNC

    public void JumpSFX(){
        jump.PlayOneShot(jumpsound);
    }

    public void MovementSFX(){
        move.PlayOneShot(movesound);
    }

    public void WaterDropSFX(){
        waterdrop.PlayOneShot(waterdropsound);
    }

    public void GanchoSFX(){
        gancho.PlayOneShot(ganchoSound);
    }

    public void RatonAtaque(){
        ratonAtaque.PlayOneShot(ratonAttkSonido);
    }

    public void TaroAtaque(){
        taroAtaque.PlayOneShot(taroAtaqueSound);
    }

    public void TaroDash(){
        taroDash.PlayOneShot(taroDashSonido);
    }

    #endregion

    #region MUSIC_FUNC

    void Update(){
/* Detecting the object that is in the radius of the detection point. */
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,
        detectionRadius, detectionLayer);

        if(obj != null){
            detectedObject = obj.gameObject;
            detectMusic = true;
            InteractMusic();

        } else {
            detectedObject = null;
            detectMusic = false;
        }
    }

    public void InteractMusic(){

        music01 = detectedObject.GetComponent<AudioSource>();

        if(detectMusic && music01.isPlaying){
            music01.Stop();
            music01.clip = musicZones[detectedObject.GetComponent<Music>().musicTrack];
        }else{
            music01.clip = musicZones[detectedObject.GetComponent<Music>().musicTrack];
        }

        
        music01.loop = true;
        music01.Play();
    }
    
    #endregion

}
