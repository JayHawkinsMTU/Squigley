using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRegionTransition : MonoBehaviour
{
    public GameObject from; // To be disabled
    public GameObject to; // To be enabled
    public bool WrapScreen = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Movement player = other.GetComponent<Movement>();
        if(player != null && player.playerID == 1)
        {
            player.ResetGrav();
            player.GetComponent<WrapScreen>().enabled = false;
            gameObject.SetActive(false);
            from.SetActive(false);
            to.SetActive(true);
        }
    }
    
}
