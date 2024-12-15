using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSinFloat : MonoBehaviour
{
    public float amplitude, frequency;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(Time.timeSinceLevelLoad * frequency) * amplitude, transform.localPosition.z);
    }
}
