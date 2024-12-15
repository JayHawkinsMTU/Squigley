using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slugfield : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float sluggishness = 2;
    Rigidbody2D rb;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;
            rb = collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(rb.velocity.x / sluggishness, rb.velocity.y / sluggishness, 0);
            rb.gravityScale = 2 / sluggishness;
            collider.gameObject.GetComponent<Movement>().maxSpeed = 10f / sluggishness;
            if(collider.gameObject.GetComponent<Movement>().movespeed > collider.gameObject.GetComponent<Movement>().maxSpeed)
            {
                collider.gameObject.GetComponent<Movement>().movespeed = collider.gameObject.GetComponent<Movement>().maxSpeed;
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            rb.gravityScale = 2;
            collider.gameObject.GetComponent<Movement>().maxSpeed = 10f;
        }
    }
}
