using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopAnimation : MonoBehaviour
{
    public string state = "idle";
    float timeCount = 0;
    int spriteIndex = 0;
    SpriteRenderer spi;
    [SerializeField] Sprite [] frames;

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

    void Idle() 
    {
        if(spriteIndex == 0 && Wait(.5f)) 
        {
            spriteIndex = 1;
            spi.sprite = frames[spriteIndex];
        }
        else if (spriteIndex == 1 && Wait(.5f))
        {
            spriteIndex = 0;
            spi.sprite = frames[spriteIndex];
        }
        else if (spriteIndex > 1)
        {
            spriteIndex = 1;
            spi.sprite = frames[spriteIndex];
        }
    }
    void Crouch()
    {
        if (spriteIndex < 1 && Wait(.1f))
        {
            spriteIndex = 1;
            spi.sprite = frames[spriteIndex];
        }
        else if (spriteIndex == 1 && Wait(.1f))
        {
            spriteIndex = 2;
            spi.sprite = frames[spriteIndex];
        }
        else if (spriteIndex > 2)
        {
            spriteIndex = 1;
            spi.sprite = frames[spriteIndex];
        }
    }
    void Jump()
    {
        if (spriteIndex < 3 && Wait(.1f))
        {
            spriteIndex = 3;
            spi.sprite = frames[spriteIndex];
        }
        else if (spriteIndex == 3 && Wait(.1f))
        {
            spriteIndex = 4;
            spi.sprite = frames[spriteIndex];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {
            case "idle":
                Idle();
                break;
            case "crouching":
                Crouch();
                break;
            case "jumping":
                Jump();
                break;
        }
    }
}
