using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    public int dir = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        SpywareBoss spyware = other.GetComponent<SpywareBoss>();
        if(spyware != null)
        {
            spyware.TurnAround(dir);
        }
    }
}
