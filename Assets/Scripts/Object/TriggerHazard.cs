using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Movement>().Die();
        }
    }
}
