using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ambience;
    [SerializeField] float volume = 0.25f;
    void Start()
    {
        audioSource.clip = ambience;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
