using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullInteractable : MonoBehaviour, Interactable
{
    AudioSource audioSource;
    [SerializeField] AudioClip interactError;
    [SerializeField] Parry parry;
    public void Interact(GameObject p)
    {
        if(parry == null || !parry.gameObject.activeInHierarchy)
        {
            audioSource.PlayOneShot(interactError, 0.5f);
        }
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
