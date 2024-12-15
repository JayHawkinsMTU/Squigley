using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public SpriteRenderer spi;
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        spi.transform.localScale = new Vector2(1f / transform.localScale.x, 1f / transform.localScale.y);
        spi.size = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Instantiate(particles, other.transform.position, Quaternion.identity);
        }
    }

}
