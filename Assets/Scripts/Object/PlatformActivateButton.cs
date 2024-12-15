using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivateButton : MonoBehaviour, Interactable
{
    public AudioSource audioSource;
    public AudioClip click;
    public ActivatedMovingBridge movingPlatform;

    // Update is called once per frame
    public void Interact(GameObject p)
    {
        audioSource.PlayOneShot(click, 0.5f);
        movingPlatform.active = true;
        movingPlatform.current = 1;
    }
}
