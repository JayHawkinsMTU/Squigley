using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == "Player")
        {
            playerCamera.GetComponent<followDaGuy>().enabled = false;
            playerCamera.transform.position = new Vector3( transform.position.x, transform.position.y, playerCamera.transform.position.z);
        }
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == "Player")
        {
            playerCamera.GetComponent<followDaGuy>().enabled = true;
        }
    }

    void Start()
    {
        if(playerCamera == null)
            playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
    }

}
