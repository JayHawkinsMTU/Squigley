using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinStretch : MonoBehaviour
{
    public float offset = 0, magnitude = 1, frequency = 1;
    public bool stretchX = false, stretchY = true;
    public Vector2 initScale;

    void Awake()
    {
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float x, y;
        if(stretchX)
        {
            x = magnitude * Mathf.Sin(offset + Time.timeSinceLevelLoad * frequency) + initScale.x;
        }
        else
        {
            x = initScale.x;
        }
        if(stretchY)
        {
            y = magnitude * Mathf.Sin(offset + Time.timeSinceLevelLoad * frequency) + initScale.y;
        }
        else
        {
            y = initScale.y;
        }
        transform.localScale = new Vector3(x, y, 1);
    }
}
