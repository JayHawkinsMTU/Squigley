using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBridge : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    public float wpRadius = 1;
    [SerializeField] bool alignOnWaypoint = false;

    void Update()
    {
        if(Vector2.Distance(waypoints[current].transform.position, transform.position) < wpRadius)
        {
            if(alignOnWaypoint)
                transform.position = waypoints[current].transform.position;
            current++;
            if(current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }
}
