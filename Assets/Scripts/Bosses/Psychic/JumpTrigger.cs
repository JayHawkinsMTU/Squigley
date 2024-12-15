using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] float jumpMultiplier = 1;
    [SerializeField] float jumpBounds = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
    
        SpywareBoss spyware = other.GetComponent<SpywareBoss>();
        if(spyware != null)
        {
            spyware.Jump(jumpBounds, jumpMultiplier);
        }
    }
}
