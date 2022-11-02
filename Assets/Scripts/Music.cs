using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int musicTrack;

    public SoundManager soundmanager;

    private void Reset(){
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 12;
    }

    // public void InteractTrack(){
    //     soundmanager.InteractMusic(gameObject);
    // }
}
