using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheosAnimation : MonoBehaviour
{
    public float amplitude = 45;
    public float frequency = 0.5f;
    private float time = 0;
    float ogY = 0;
    // Start is called before the first frame update
    void Start()
    {
        ogY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, ogY + Mathf.Sin(frequency * time) * amplitude, transform.position.z);
    }
}
