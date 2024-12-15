using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : MonoBehaviour
{
    const float MIN_DISTANCE = 40f;
    const float WAIT_TIME = 3;
    const float CATCH_UP_RELIEF = 3;
    public float moveSpeed = 1;
    private float initZ;
    [SerializeField] GameObject player;
    Wait cooldown = new Wait(WAIT_TIME);
    void Move()
    {
        if(player.transform.position.y > transform.position.y + MIN_DISTANCE)
        {
            transform.position = new Vector3(0, player.transform.position.y - MIN_DISTANCE + CATCH_UP_RELIEF, initZ);
        }
        transform.position = new Vector3(0, transform.position.y + moveSpeed * Time.deltaTime, initZ);
    }
    public void Reset()
    {
        cooldown.Reset();
    }
    void Start()
    {
        initZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown.isDone())
        {
            Move();
        }
        else
        {
            cooldown.Iterate();
        }
    }
}
