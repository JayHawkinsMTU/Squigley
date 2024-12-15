using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseInteractTrigger : MonoBehaviour
{
    NullInteractable nullInteractable;
    public Vase interactable;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = interactable;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractKey>().interactable = nullInteractable;
        }
    }
    void Start()
    {
        nullInteractable = GameObject.Find("EventSystem").GetComponent<NullInteractable>();
    }
}
