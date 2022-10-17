using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerIA : MonoBehaviour
{
    public Rigidbody2D RB;

    public void Awake(){
        RB = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
