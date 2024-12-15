using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedMovingBridge : MonoBehaviour
{
    public GameObject[] waypoints;
    public int current = 0;
    float rotSpeed;
    public float speed;
    public float wpRadius = 1;
    public bool active = false;
    [SerializeField] bool returnToStart = true;
    [SerializeField] bool alignOnWaypoint = false;
    PlatformAttach pa;

    void Start()
    {
        pa = GetComponent<PlatformAttach>();
        active = false;
    }

    void Update()
    {
        if(pa.attached == true)
        {
            active = true;
        }
        if(active)
        {
            if(Vector2.Distance(waypoints[current].transform.position, transform.position) < wpRadius)
            {
                if(alignOnWaypoint)
                    transform.position = waypoints[current].transform.position;
                current++;
                //Stops platform when it reaches it's starting position. :)
                if(current == 1)
                {
                    active = false;
                }
                if(current >= waypoints.Length)
                {
                    current = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
        }
        else if(returnToStart && current != 0 && Vector2.Distance(waypoints[0].transform.position, transform.position) > wpRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, Time.deltaTime * speed);
        }
    }
}
