using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgInteractTrigger : MonoBehaviour
{
    NullInteractable nullInteractable;
    public NewMessageHandler interactable;
    public bool inTrigger = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Movement>() != null)
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = interactable;
            inTrigger = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Movement>() != null)
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
