using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        SpywareBoss spyware = other.GetComponent<SpywareBoss>();
        if(spyware != null)
        {
            spyware.Chase();
        }
    }
}
