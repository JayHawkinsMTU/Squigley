using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRBTrigger : MonoBehaviour
{
    public Rigidbody rb;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            rb.angularVelocity = new Vector3(0, -.5f * other.GetComponent<Rigidbody2D>().velocity.x, 0);
            Debug.Log(rb.angularVelocity);
        }
    }
}
