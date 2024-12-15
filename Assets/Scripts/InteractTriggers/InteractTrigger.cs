using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    NullInteractable nullInteractable;
    public InteractableC interactable;
    public bool inTrigger = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Cast doesn't work. This endeavor is a bust.
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = (Interactable) interactable;
            inTrigger = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = nullInteractable;
            inTrigger = false;
        }
    }
    void Start()
    {
        nullInteractable = GameObject.Find("EventSystem").GetComponent<NullInteractable>();
    }
}

