using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : Parryable
{
    float speed = 3.5f;
    float velocityX= 1f;
    float velocityY = 1f;
    private Wait turnWait = new Wait(.5f);
    //Trigger exit correction
    [System.NonSerialized] public BatCage cage;
    [System.NonSerialized] public bool inCage = true;
    [System.NonSerialized] public Vector2 exitPoint; //The point to teleport back to. 
    private const float CORRECTION_TIME = 2f; //Time it takes to teleport to exitPoint when out of cage
    private const float INVINC_TIME = 3.5f; //The time for hitbox to be disabled when failsafe is used.
    private Wait corWait = new Wait(CORRECTION_TIME);
    private Wait invWait = new Wait(INVINC_TIME);
    private bool failSafe; //For moving cages. If exit point doesn't return them to the cage, the bat should teleport to the center of the cage. not ideal but it is what it is.
    private BoxCollider2D hitbox;
    
    public void Reverse()
    {
        turnWait.Reset();
        turnWait = new Wait(Random.Range(.05f, 1f));
        velocityX = -1 * velocityX;
        velocityY = -1 * velocityY;
    }
    public void ChangeDirection(float dir)
    {
        turnWait.Reset();
        turnWait = new Wait(Random.Range(.25f, 1f));
        velocityX = Mathf.Cos(dir) * speed;
        velocityY = Mathf.Sin(dir) * speed;
    }
    float RandomDir() //Random radian between 0 and 2pi.
    {
        return Random.Range(0, 2 * Mathf.PI);
    }
    public float Towards(Vector2 point) //Finds the angle in radian towards a point.
    {
        float deltaX = point.x - transform.position.x;
        float deltaY = point.y - transform.position.y;
        return Mathf.Atan2(deltaY, deltaX);
    }
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        exitPoint = transform.position;
        ChangeDirection(RandomDir());
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            Reverse();
        }
    }
    // Destory upon parry
    public override void Parry()
    {
        Destroy(this.gameObject);
    }
    void CageCorrection()
    {
        corWait.Iterate();
        if(corWait.Complete())
        {
            corWait.Reset();
            if(!failSafe)
            {
                transform.position = exitPoint;
                failSafe = true;
            }
            else
            {
                transform.position = cage.gameObject.transform.position;
                failSafe = false;
            }
            ChangeDirection(Towards(cage.gameObject.transform.position));
        }
    }
    void FailSafeInv()
    {
        invWait.Iterate();
        if(invWait.Complete())
        {
            invWait.Reset();
            hitbox.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = new Vector3(transform.localPosition.x + velocityX * Time.deltaTime, transform.localPosition.y + velocityY * Time.deltaTime);
        transform.Translate(velocityX * Time.deltaTime, velocityY * Time.deltaTime, 0);
        turnWait.Iterate();
        if(turnWait.Complete())
        {
            ChangeDirection(RandomDir());
            turnWait.Reset();
        }
        //Cage correction
        if(!inCage)
        {
            CageCorrection();
        }
        else
        {
            failSafe = false;
        }

        //Failsafe hitbox handling
        if(!hitbox.enabled)
        {
            FailSafeInv();
        }
    }
}
