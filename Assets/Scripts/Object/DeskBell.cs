using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskBell : Box
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] MoveTo moveTo;
    public override void Interact(GameObject p)
    {
        
        audioSource.PlayOneShot(audioClip, 0.5f);
        moveTo.enabled = true;
    }
}
