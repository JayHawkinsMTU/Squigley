using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotate : MonoBehaviour
{
    public float amplitude = 45;
    public float frequency = 0.5f;
    public float timeOffset = 0;
    private float time = 0;
    private float initZ;
    void Awake()
    {
        initZ = transform.eulerAngles.z;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, initZ + Mathf.Sin(frequency * (time + timeOffset)) * amplitude);
    }
}
