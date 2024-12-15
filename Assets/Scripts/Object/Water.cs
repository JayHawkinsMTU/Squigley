using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject particle;
    [SerializeField] float sluggishness = 2;
    Rigidbody2D waterRigidbody;
    AudioSource audioSource;
    [SerializeField] AudioClip splash;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;
            waterRigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
            if(waterRigidbody.velocity.y < -15)
            {
                audioSource.PlayOneShot(splash, 0.5f);
                particle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.25f);
                particle.GetComponent<ParticleSystem>().Play();
            } 
            waterRigidbody.velocity = new Vector3(waterRigidbody.velocity.x / sluggishness, waterRigidbody.velocity.y / sluggishness, 0);
            waterRigidbody.gravityScale = -2 / sluggishness;
            collider.gameObject.GetComponent<Movement>().maxSpeed = 10f / sluggishness;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            waterRigidbody.gravityScale = 2;
            collider.gameObject.GetComponent<Movement>().maxSpeed = 10f;
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
