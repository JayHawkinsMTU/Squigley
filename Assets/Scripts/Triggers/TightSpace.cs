using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightSpace : MonoBehaviour
{
    GameObject player;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            //Locks player scale and rotation to behave consistently in tight spaces.
            if(player.TryGetComponent<Movement>(out Movement movement)) {
                movement.inTightSpace = false;
            }
            player.transform.localScale = new Vector3(.5f, 1.2f, player.transform.localScale.z);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(player.TryGetComponent<Movement>(out Movement movement)) {
            movement.inTightSpace = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
}
