using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapScreen : MonoBehaviour
{
    public float upperBound = 15f;
    public float lowerBound = -15f;
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > upperBound)
        {
            transform.position = new Vector2(lowerBound, transform.position.y);
        }
        else if(transform.position.x < lowerBound)
        {
            transform.position = new Vector2(upperBound, transform.position.y);
        }
    }
}
