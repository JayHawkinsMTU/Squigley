using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncrouchCheck : MonoBehaviour
{
    [SerializeField] Movement movement;
    public bool uncrouchCheck = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            //Debug.Log("Can't uncrouch");
            uncrouchCheck = false;
            movement.CoyoteDisable();
        }
    }

    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "Environment")
        {
            //Debug.Log("Can't uncrouch");
            uncrouchCheck = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            //Debug.Log("Can't uncrouch");
            uncrouchCheck = true;
        }
    }
}
