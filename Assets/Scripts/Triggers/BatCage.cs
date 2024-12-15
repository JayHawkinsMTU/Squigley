using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCage : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collider)
    {
        BatMovement bat = collider.gameObject.GetComponent<BatMovement>();
        if(bat != null)
        {
            bat.cage = this;
            bat.exitPoint = bat.transform.position;
            bat.inCage = false;
            bat.ChangeDirection(bat.Towards(transform.position));
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        BatMovement bat = collider.gameObject.GetComponent<BatMovement>();
        if(bat != null)
        {
            bat.cage = this;
            bat.inCage = true;
        }
    }
}
