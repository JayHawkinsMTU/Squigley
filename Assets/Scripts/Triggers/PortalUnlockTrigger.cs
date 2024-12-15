using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalUnlockTrigger : MonoBehaviour
{
    public LockedPortal associatedPortal;
    public LockedPortal requiredPortal;
    [SerializeField] EnableVisualEvent visual;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(associatedPortal.unlocked)
            {
                Destroy(this);
            }
            else if(requiredPortal == null || requiredPortal.unlocked)
            {
                associatedPortal.Unlock();
                if(visual != null)
                {
                    visual.Event(this.gameObject);
                }
                Destroy(this);
            }
        }
    }
    void Start()
    {
        if(associatedPortal.unlocked)
        {
            Destroy(this);
        }
    }
}
