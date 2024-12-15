using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject linkedPortal;
    public AudioSource audioSource;
    [SerializeField] AudioClip warp;
    public bool active = false;
    private float cooldown = 1f;
    Wait wait;

    void Start()
    {
        wait = new Wait(cooldown);
        if(linkedPortal != null) audioSource = linkedPortal.GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!active /*&& !linkedPortal.GetComponent<Portal>().active*/ && collision.gameObject.tag == "Player")
        {
            active = true;
            linkedPortal.GetComponent<Portal>().active = true;
            audioSource.PlayOneShot(warp, 0.75f);
            collision.gameObject.transform.position = new Vector3(linkedPortal.transform.position.x, linkedPortal.transform.position.y, collision.gameObject.transform.position.z);
        }
    }
    void Update()
    {
        if(active)
        {
            wait.Iterate();
            if(wait.Complete())
            {
                wait = new Wait(cooldown);
                active = false;
            }
        }
    }
}
