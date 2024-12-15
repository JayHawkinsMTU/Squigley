using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorButton : MonoBehaviour, Interactable
{
    [SerializeField] bool toggleable;
    [SerializeField] LockedDoor door;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    public void Interact(GameObject p)
    {
        if(toggleable)
        {
            if(door.unlocked)
            {
                door.Lock();
            }
            else
            {
                door.Unlock();
            }
        }
        else
        {
            door.Unlock();
            audioSource.PlayOneShot(audioClip, 0.5f);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
