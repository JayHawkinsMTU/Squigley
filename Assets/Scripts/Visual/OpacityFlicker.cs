using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityFlicker : MonoBehaviour
{
    SpriteRenderer spi;
    [SerializeField] float minBound;
    [SerializeField] float maxBound;
    Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        startColor = spi.color;
    }

    // Update is called once per frame
    void Update()
    {
        spi.color = new Color(startColor.r, startColor.g, startColor.b, Random.Range(minBound, maxBound));
    }
}
