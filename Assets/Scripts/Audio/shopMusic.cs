using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopMusic : MonoBehaviour
{
    public GameObject messagebox;
    public AudioSource player;
    public AudioSource audioSource;
    NewMessageHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<AmbientSound>().audioSource;
        handler = GetComponent<NewMessageHandler>();
        //audioSource.Stop();
    }
    public void StartMusic()
    {
        player.Stop();
        audioSource.Play();
    }
    public void StopMusic()
    {
        audioSource.Stop();
        player.Play();
    }
}