using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteractTrigger : MonoBehaviour
{
    NullInteractable nullInteractable;
    public Box interactable;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Movement>() != null)
        {
            interactable.player = collision.gameObject;
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = interactable;
            interactable.playerID = collision.gameObject.GetComponent<Movement>().playerID;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Movement>() != null)
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = nullInteractable;
        }
    }
    void Start()
    {
        nullInteractable = GameObject.Find("EventSystem").GetComponent<NullInteractable>();
    }
}
