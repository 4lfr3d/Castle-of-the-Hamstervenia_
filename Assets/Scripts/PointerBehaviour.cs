// Script basado en uno de Next Wave 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour
{
    public Transform pivote;
    [SerializeField] private float hideDistance;

    void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = target - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // this.transform.RotateAround(pivote.transform.position, Vector3.forward, angle);

        if(direction.magnitude < hideDistance){
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else{
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}