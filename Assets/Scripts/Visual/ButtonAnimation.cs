using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    //public MessageHandler textBoxScript;
    public GameObject button;
    int timeCount = 0;
    SpriteRenderer spi;
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeCount >= 16)
        {
            //Resets animation timer
            timeCount = 0;
        }
        //Rotates sprite
        if(timeCount == 0)
        {
            spi.flipY = false;
            spi.flipX = false;
        }
        else if(timeCount == 4)
        {
            spi.flipX = true;
        }
        else if(timeCount == 8)
        {
            spi.flipY = true;
        }
        else if(timeCount == 12)
        {
            spi.flipX = false;
        }
        //Flickers sprite
        if( timeCount % 2 == 0)
        {
            spi.enabled = false;
        }
        else
        {
            spi.enabled = true;
        }
        //Adds count to time
        timeCount++;
        //Colors sprite when there is a new message.
        if(Message.newMessage)
        {
            spi.color = new Color(255,255,0);
        }
        else
        {
            spi.color = new Color(255,255,255);
        }
    }
}
