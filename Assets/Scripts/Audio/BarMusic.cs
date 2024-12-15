using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMusic : MonoBehaviour
{
    public AmbientSound player;
    public AudioSource audioSource;
    bool musicPlaying = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !musicPlaying)
        {
            //Debug.Log("Swapping to music");
            player.audioSource.Stop();
            audioSource.Play();
            musicPlaying = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            player.audioSource.Stop();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && musicPlaying)
        {
            //Debug.Log("Swapping to ambience");
            audioSource.Stop();
            player.audioSource.Play();
            musicPlaying = false;
        }
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<AmbientSound>();
    }
}