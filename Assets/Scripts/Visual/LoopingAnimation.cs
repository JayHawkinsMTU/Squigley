using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAnimation : MonoBehaviour
{
    [SerializeField] float fps = 1;
    int frameIndex;
    public SpriteRenderer spi;
    [SerializeField] Sprite[] frames;
    [SerializeField] bool loop = true;
    Wait wait;

    // Start is called before the first frame update
    void Awake()
    {
        wait = new Wait(1 / fps);
        if(spi == null) spi = GetComponent<SpriteRenderer>();
        
    }
    void OnEnable()
    {
        spi = GetComponent<SpriteRenderer>();
        Reset();
    }
    void NextFrame()
    {
        frameIndex++;
        spi.sprite = frames[frameIndex];
    }
    public void Reset()
    {
        frameIndex = 0;
        spi.sprite = frames[frameIndex];
    }

    // Update is called once per frame
    void Update()
    {
        wait.Iterate();
        if(wait.Complete())
        {
            if(frameIndex < frames.Length - 1) NextFrame();
            else
            {
                if(loop)
                {
                    Reset();
                }
                else
                {
                    enabled = false;
                }

            } 
        }
    }
}
