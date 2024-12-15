using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySync : MonoBehaviour
{
    private Rigidbody2D thisrb;
    private Rigidbody2D playerrb;
    private float baseGravity;

    void Start()
    {
        thisrb = GetComponent<Rigidbody2D>();
        baseGravity = thisrb.gravityScale;
        playerrb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(playerrb.gravityScale < 0)
        {
            thisrb.gravityScale = baseGravity * -1;
        }
        else
        {
            thisrb.gravityScale = baseGravity;

        }
    }
}
