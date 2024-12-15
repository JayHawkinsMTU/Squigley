using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelMovement : MonoBehaviour
{
    public float speed, frequency, amplitude, attentionSpan;
    public Vector2 lowerLeft, upperRight; // Bounds for random location selection
    private Vector2 to; // The point that the angel is moving to.
    private Vector2 current;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        NewSpot();
        current = transform.position;
    }

    void NewSpot()
    {
        to = new Vector2(Random.Range(lowerLeft.x, upperRight.x), Random.Range(lowerLeft.y, upperRight.y));
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        current = Vector2.MoveTowards(current, to, speed * Time.deltaTime);
        if(to.x > current.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        transform.position = new Vector3(current.x, current.y + amplitude * Mathf.Sin(time * frequency), transform.position.z);
        if(Vector2.Distance(current, to) < 1.5f)
        {
            NewSpot();
        }
    }
}
