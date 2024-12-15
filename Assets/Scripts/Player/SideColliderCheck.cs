using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideColliderCheck : MonoBehaviour
{
    private const float VEL_THRESH = .5f;
    private const float DELAY = .1f; //How much time it takes for isTouching to turn back to false when true.
    public bool isTouching = false;
    private bool release = true;
    Wait releaseWait = new Wait(DELAY);
    [SerializeField] Rigidbody2D playerRigidBody;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Trigger" && collision.gameObject.tag != "BatCage" && !collision.gameObject.CompareTag("Hazards"))
        {
            if(playerRigidBody != null)
                playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            isTouching = true;
            release = false;
            releaseWait.Reset();
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Trigger" && collision.gameObject.tag != "BatCage" && collision.gameObject.tag != "Hazards")
        {
            isTouching = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Trigger" && collision.gameObject.tag != "BatCage" && collision.gameObject.tag != "Hazards")
        {
            isTouching = false;
        }
    }
    void Release()
    {
        if(release && isTouching)
        {
            releaseWait.Iterate();
            if(releaseWait.Complete())
            {
                isTouching = false;
                release = false;
            }
        }
    }
    void Update()
    {
        //JumpScale();
        Release();
    }
}
