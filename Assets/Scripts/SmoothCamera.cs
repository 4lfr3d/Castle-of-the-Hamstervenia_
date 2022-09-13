/*
    Code By bendux - https://www.youtube.com/watch?v=ZBj3LBA2vUY
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 offset = new Vector3(50f,50f,-250f);

    private float smoothTime = 0.25f;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private void Update() {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);    
    }
}
