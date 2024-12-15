using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCamera : MonoBehaviour
{
    private float initY; // Starting y position
    public float minX, maxX; // Constraints to x position, based locally.
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(Mathf.Clamp(player.transform.position.x - transform.parent.position.x, minX, maxX), initY, transform.position.z);
    }
}
