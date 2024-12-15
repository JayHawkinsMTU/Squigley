using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffect : MonoBehaviour
{
    public const int FLASHBLINK = 0;
    public const int FADEIN = 1;
    public const int FADEOUT = 2;
    [SerializeField] bool fadeFromStart = false;
    private int mode = 0;
    private float length = 1f;
    private Wait flashBlinkWait = new Wait(0.1f); //5 times per second.
    private Wait lengthWait = new Wait(1f);
    Image image;
    public virtual void StartEffect(float l, int m)
    {
        this.gameObject.SetActive(true);
        this.mode = m;
        this.length = l;
        lengthWait = new Wait(length);
        if(m == FADEOUT)
            image.color = new Color(image.color.r, image.color.g, image.color.b , 1); //Sets max opacity.
    }
    void Start()
    {
        image = GetComponent<Image>();
        if(fadeFromStart)
            StartEffect(5,2);
    }
    void Update()
    {
        lengthWait.Iterate();
        if(lengthWait.Complete())
        {
            lengthWait.Reset();
            flashBlinkWait.Reset();
            if(mode != FADEIN)
                this.gameObject.SetActive(false);
        }
        switch(mode)
        {
            case FLASHBLINK:
                flashBlinkWait.Iterate();
                if(flashBlinkWait.Complete())
                {
                    flashBlinkWait.Reset();
                    image.enabled = !image.enabled;
                }
                break;
            case FADEIN:
                if(image.color.a < 1)
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime / length);
                break;
            case FADEOUT:
                if(image.color.a > 0)
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime / length);
                break;
            default:
                Debug.LogError("Invalid visual effect mode. 0: flashblink, 1: fadein, 2: fadeout");
                break;
        }
    }
}
