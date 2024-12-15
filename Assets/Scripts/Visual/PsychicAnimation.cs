using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicAnimation : MonoBehaviour
{
    public SpriteRenderer spi;
    public Sprite [] frames;
    int currentFrame = 0;
    float timeCount = 0;
    [SerializeField] float spf = .5f;
    bool animationRunning = false;
    float randTime = 0;
    public ParticleSystem smoke;

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
        randTime = Random.Range(5f, 11f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!animationRunning)
        {
            if(Wait(randTime))
            {
                animationRunning = true;
                randTime = Random.Range(5f, 11f);
            }
        }
        else
        {
            if(Wait(spf))
            {
                if(currentFrame < frames.Length - 1)
                {
                    currentFrame++;
                    spi.sprite = frames[currentFrame];
                    if(currentFrame == 3)
                    {
                        animationRunning = false; //Pauses animation as psychic takes a smoke.
                        randTime = Random.Range(1f,5f);
                    }
                    else if(currentFrame == 4)
                    {
                        smoke.Play();
                    }
                }
                else
                {
                    currentFrame = 0;
                    spi.sprite = frames[currentFrame];
                    animationRunning = false;
                }
            }
        }
    }
}
