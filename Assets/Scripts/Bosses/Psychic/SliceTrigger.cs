using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceTrigger : MonoBehaviour
{
    [SerializeField] SpywareBoss spyware;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            spyware.ActivateSlice();
        }
    }
}
