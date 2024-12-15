using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedPlatInteractTrigger : MonoBehaviour
{
    NullInteractable nullInteractable;
    public PlatformActivateButton interactable;
    public bool inTrigger = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = interactable;
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
