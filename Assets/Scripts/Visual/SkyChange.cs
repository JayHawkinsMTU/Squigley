using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyChange : MonoBehaviour
{
    public SpriteRenderer spi;
    public float fadeInSpeed;
    public int status = 0;

    void FadeIn()
    {
        if(spi.color.a < 1)
        {
            spi.color = new Color(1f,1f,1f, spi.color.a + Time.deltaTime * fadeInSpeed);
        }
        else
        {
            spi.color = new Color(1f,1f,1f,1f);
        }
    }
    void FadeOut()
    {
        if(spi.color.a > 0)
        {
            spi.color = new Color(1f,1f,1f, spi.color.a - Time.deltaTime * fadeInSpeed);
        }
        else
        {
            spi.color = new Color(1f,1f,1f,0f);
        }
    }
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        switch(status)
        {
            case 0:
                break;
            case -1:
                FadeOut();
                break;
            case 1:
                FadeIn();
                break;
        }
    }
}
