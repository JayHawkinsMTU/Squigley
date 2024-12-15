using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerTrigger : MonoBehaviour
{
    public GameObject pointer;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            pointer.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            pointer.SetActive(false);
        }
    }
}
