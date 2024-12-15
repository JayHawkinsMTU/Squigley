using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimation : MonoBehaviour
{
    public SpriteRenderer spi;
    public Sprite [] frames;
    int currentFrame = 0;
    float timeCount = 0;
    [SerializeField] float spf = .025f;

    bool Wait(float time)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= time)
        {
            timeCount = 0;
            return true;
        }
        else return false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Wait(spf))
        {
            if(currentFrame >= frames.Length - 1)
            {
                currentFrame = 0;
            }
            currentFrame++;
            spi.sprite = frames[currentFrame];
            if(currentFrame == 3 || currentFrame == 0 || currentFrame == 9)
            {
                spi.flipY = !spi.flipY;
            }
        }
    }
}
