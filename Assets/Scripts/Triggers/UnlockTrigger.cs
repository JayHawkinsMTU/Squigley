using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTrigger : MonoBehaviour
{
    public LockedDoor associatedDoor;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            associatedDoor.Unlock();
            Destroy(this);
        }
    }
    void Start()
    {
        if(associatedDoor.unlocked)
        {
            Destroy(this);
        }
    }
}
