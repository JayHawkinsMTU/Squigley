using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public bool attached = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && other.transform.parent == null) //Other parent must be null in order to prevent other platforms taking player away.
        {
            //Debug.Log("Attach");
            other.transform.parent = transform;
            attached = true;
            other.GetComponent<Movement>().physicalMovement = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Detach");
            if(!TryGetComponent<Movement>(out Movement m)) other.transform.parent = null;
            attached = false;
        }
    }
}
