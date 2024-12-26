using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Corruption : MonoBehaviour
{
    const float MIN_DISTANCE = 40f;
    const float WAIT_TIME = 3;
    const float CATCH_UP_RELIEF = 3;
    public float moveSpeed = 1;
    private Vector3 initPos;
    [SerializeField] Movement player;

    private IEnumerator Lifecycle()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        // Move until player dies, then fill screen and stop.
        while(!player.dead || player.transform.position.y > transform.position.y)
        {
            Move();
            yield return new WaitForEndOfFrame();
        }
    }

    void Move()
    {
        if(player.transform.position.y > transform.position.y + MIN_DISTANCE)
        {
            transform.position = new Vector3(
                0, player.transform.position.y - MIN_DISTANCE + CATCH_UP_RELIEF, initPos.z
            );
        }
        transform.position = new Vector3(
            0, transform.position.y + moveSpeed * Time.deltaTime, initPos.z
        );
    }
    public void Reset()
    {
        transform.position = initPos;
        StopAllCoroutines();
        StartCoroutine(Lifecycle());
    }
    void Start()
    {
        initPos = transform.position;
        StartCoroutine(Lifecycle());
    }
}
