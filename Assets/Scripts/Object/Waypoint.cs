using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint[] waypoints;

    public Waypoint GetRandomAdjacent()
    {
        if(waypoints.Length == 0) {
            Debug.LogError(gameObject.name + " has no assigned adjacent waypoints");
            return null;
        }
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        /*for(int i = 0; i < waypoints.Length; i++)
        {
            if(waypoints[i] == null) {
                Debug.LogError(gameObject.name + " has unassigned adjacent waypoints");
            }
        }*/
    }
}
