using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    [SerializeField] GameObject region;
    public int playersInRegion;
    private bool regionActive = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playersInRegion++;
            if(!regionActive)
            {
                regionActive = true;
                region.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("Player left region: " + region.name);
            playersInRegion--;
            if(playersInRegion < 1 && regionActive)
            {
                regionActive = false;
                if(region == null) 
                {
                    enabled = false; //Load trigger no longer useful if object is destroyed. Used for spyware boss.
                    return;
                }
                region.SetActive(false);
            }
            
        }
    }
}
