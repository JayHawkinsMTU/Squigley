using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrigger : MonoBehaviour
{
    SkyChange skyChange;
    [SerializeField] Sprite image;
    [SerializeField] float fadeInSpeed = 1f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            skyChange.spi.sprite = image;
            skyChange.fadeInSpeed = fadeInSpeed;
            skyChange.status = 1;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            skyChange.status = -1;
        }
    }
    void Start()
    {
        skyChange = GameObject.Find("BG Image").GetComponent<SkyChange>();
    }
}
