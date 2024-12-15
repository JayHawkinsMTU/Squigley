using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followDaGuy : MonoBehaviour
{
    const float MAX_SPEED = 150f;
    const float SMOOTHNESS = 0.5f;
    const float CATCH_UP_DISTANCE = 150f;
    float speed;
    public bool smoothCamera = false;
    [SerializeField] float verticalShift = 1f;
    [SerializeField] GameObject player;
    [SerializeField] bool lockX = false;
    [SerializeField] float lockXTo = 0;

    // Update is called once per frame
    public void UpdateCamera()
    {
        if(!enabled)
        {
            return;
        }
        if(!lockX)
        {
            if(!smoothCamera)
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + verticalShift, -10);
            }
            else
            {
                Smoothcam();
            }

        }
        else
        {
            transform.position = new Vector3(lockXTo, player.transform.position.y + verticalShift, -10);
        }
    }
    void Smoothcam()
    {
        float distance = Vector2.Distance(this.transform.position, player.transform.position);
        if(distance < CATCH_UP_DISTANCE)
            speed = Mathf.Clamp(distance * (1 / SMOOTHNESS), 0, MAX_SPEED);
        else // Teleport to the point where the distance between the camera and player is equal to CATCH_UP_DISTANCE
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, Vector2.Distance(this.transform.position, player.transform.position) - CATCH_UP_DISTANCE);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10); //Corrects z position and y offset.
    }

    public void InstantToPlayer()
    {
        if(smoothCamera) transform.position = player.transform.position;
    }
    void FixedUpdate()
    {
        if(smoothCamera) UpdateCamera();
    }
}
