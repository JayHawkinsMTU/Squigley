using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedfield : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float boost = 2;
    Rigidbody2D rb;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;
            rb = collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(rb.velocity.x * boost, rb.velocity.y/* * boost*/, 0);
            collider.gameObject.GetComponent<Movement>().maxSpeed = 10f * boost;
            collider.gameObject.GetComponent<Movement>().acceleration = 20f * boost;
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
            collider.gameObject.GetComponent<Movement>().acceleration = 20f;
        }
    }
}
