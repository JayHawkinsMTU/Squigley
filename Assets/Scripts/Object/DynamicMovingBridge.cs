using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicovingBridge : MonoBehaviour
{
    public Waypoint waypoint;
    float rotSpeed;
    public float speed;
    public float wpRadius = 1;
    [SerializeField] bool alignOnWaypoint = false;

    void Awake()
    {
        SpriteRenderer spi;
        if(TryGetComponent<SpriteRenderer>(out spi)) 
        {
            spi.enabled = false;
        }
    }
    void Update()
    {
        
        if(Vector2.Distance(waypoint.transform.position, transform.position) < wpRadius)
        {
            if(alignOnWaypoint)
            {
                transform.position = waypoint.transform.position;
            }
            string prev = waypoint.name;
            waypoint = waypoint.GetRandomAdjacent();
            if(waypoint == null) {
                Debug.LogError("Dynamic moving bridge: " + gameObject.name + " has a null waypoint");
                enabled = false;
            }
            
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, Time.deltaTime * speed);
    }
}
