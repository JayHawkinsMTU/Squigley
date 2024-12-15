using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilroyHealthBar : MonoBehaviour
{
    [SerializeField] KilroyVulnerability kv;
    SpriteRenderer spi;
    Wait blinkWait = new Wait(0.05f);
    Color initColor;
    float initSizeX;
    float nextSize;
    int startLives;
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        initSizeX = transform.localScale.x;
        initColor = spi.color;
        startLives = kv.lives;
        nextSize = initSizeX;
    }

    public void Reset()
    {
        transform.localScale = new Vector3(initSizeX, transform.localScale.y, 0);
        spi.color = initColor;
        nextSize = initSizeX;
    }

    public void Damage()
    {
        nextSize = initSizeX * kv.lives / startLives;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > nextSize)
        {
            transform.localScale = new Vector3(transform.localScale.x - 3 * Time.deltaTime, transform.localScale.y, 0);
            blinkWait.Iterate();
            if(blinkWait.Complete())
            {
                if(spi.color == initColor)
                {
                    spi.color = Color.white;
                } else {
                    spi.color = initColor;
                }
            }
        } else if(transform.localScale.x > nextSize)
        {
            transform.localScale = new Vector3(nextSize, transform.localScale.y, 0);
            spi.color = initColor;
        } else if(spi.color != initColor) {
            spi.color = initColor;
        }
    }
}
