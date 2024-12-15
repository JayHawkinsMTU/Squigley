using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableVisualEvent : MessageEvent
{
    [SerializeField] float length;
    [SerializeField] int mode; //Should be 0 or 1. More modes may come later.
    [SerializeField] VisualEffect visual;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Event(GameObject p)
    {
        visual.StartEffect(length, mode);
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, 0.6f);
        }
    }
}
