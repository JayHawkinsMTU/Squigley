using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLaser : MonoBehaviour
{
    const float FLASH_LENGTH = 0.025f; // 3-30 Hz is considered sensitive to epilepsy. This is either 20 - 40 Hz. Adjust red to make less drastic.
    const float GROWTH_RATE = 5; // Rate of growth for laser, takes 1/5 of a second to get to max size
    [SerializeField] float size = 3;
    [SerializeField] float length = 3;
    private float ratio = 0; // Ratio of current scale to max size.
    private BoxCollider2D hitbox;
    private SpriteRenderer spi;
    private Color initColor;


    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        spi = GetComponent<SpriteRenderer>();
        initColor = spi.color;
        StartCoroutine(Open());
    }

    // Grows laser
    IEnumerator Open()
    {
        StartCoroutine(Close());
        StartCoroutine(Flash());
        while(ratio < 1)
        {
            ratio += GROWTH_RATE * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, ratio * size, transform.localScale.z);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = new Vector3(transform.localScale.x, size, transform.localScale.z);
        hitbox.enabled = true;
    }

    // Shrinks then destroys laser
    IEnumerator Close()
    {
        yield return new WaitForSeconds(length);
        StopCoroutine(Open());
        while(ratio > 0)
        {
            ratio -= GROWTH_RATE * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, ratio * size, transform.localScale.z);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
    IEnumerator Flash()
    {
        while(true)
        {
            if(spi.color == initColor)
            {
                spi.color = Color.white;
            }
            else
            {
                spi.color = initColor;
            }
            yield return new WaitForSeconds(FLASH_LENGTH);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
