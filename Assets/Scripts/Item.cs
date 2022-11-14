using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public InventorySystem invsys;

    
    public enum InteractionType {Default, PickUp}

    public enum ItemType {Static, Consumables}

    [Header("Attributes")]
    public InteractionType type;
    public ItemType item_type;
    public string item_desc;
    public bool stackable = false;
    public int count = 1;

    [Header("Custom Events")]
    public UnityEvent consumeEvent;

    void Update(){
        if(invsys == null){
            invsys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        }
    }
    
    private void Reset(){
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 8;
    }

    public void Interact(){

        switch(type)
        {
            case InteractionType.PickUp:
                
                if(!invsys.CanPickUp()){
                    return;
                }
                invsys.PickUp(gameObject);
                Debug.Log("PickUp");

                gameObject.SetActive(false);
                break;
            default :
                Debug.Log("default");
                break;
        }

        consumeEvent.Invoke();
    }
}
