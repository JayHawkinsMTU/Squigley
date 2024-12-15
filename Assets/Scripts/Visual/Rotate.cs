using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 5; // Deg per second
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Time.timeSinceLevelLoad * speed);
    }
}
