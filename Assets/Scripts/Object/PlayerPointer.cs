using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointer : MonoBehaviour
{
    GameObject player;
    public float blinkHZ = 5;
    SpriteRenderer spi;
    private float maxA;
    private float minA = 0.1f;

    void Awake()
    {
        player = GameObject.Find("Player");
        spi = GetComponent<SpriteRenderer>();
        maxA = spi.color.a;
    }

    void OnEnable()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        //Follows player on x axis
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }

    IEnumerator Blink()
    {
        while(true)
        {
            if(spi.color.a == maxA) spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, minA);
            else spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, maxA);
            yield return new WaitForSeconds(1f / blinkHZ);
        }
    }
}
