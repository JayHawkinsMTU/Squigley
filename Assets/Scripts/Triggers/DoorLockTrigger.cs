using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockTrigger : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip lockSound;
    public GameObject[] associatedDoors;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            for(int i = 0; i < associatedDoors.Length; i++)
            {
                associatedDoors[i].GetComponent<LockedDoor>().Lock();
            }
            audioSource.PlayOneShot(lockSound, 0.5f);
        }
    }
}
