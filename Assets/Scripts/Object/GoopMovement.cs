using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopMovement : Parryable
{
    string state = "landed";
    [SerializeField] float jumpForceX = 200f;
    [SerializeField] float jumpForceY = 300f;

    GoopAnimation gAnimation;
    Rigidbody2D rb;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioClip boing;
    float timeCount = 0f;
    private float rest = 0.5f;
    int sign = 1;
    public SideColliderCheck leftSideColliderCheck;
    public SideColliderCheck rightSideColliderCheck;
    public GameObject greenGoop;



    bool Wait(float time)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= time)
        {
            timeCount = 0;
            return true;
        }
        else return false;
    }
    void Reverse()
    {
        sign *= -1;
        rb.velocity = new Vector3(-rb.velocity.x, -rb.velocity.y, 0f);
    }
    void Jump()
    {   
        if(state != "jumping")
        {
            timeCount = 0;
            state = "jumping";
        }
        if(gAnimation.state != "jumping")
        {
            gAnimation.state = "crouching";
        }
        if(Wait(rest) && gAnimation.state != "jumping")
        {
            gAnimation.state = "jumping";
            audioSource.PlayOneShot(audioClip, 0.75f);
            rb.AddForce(transform.right * jumpForceX * sign);
            rb.AddForce(transform.up * jumpForceY);
        }
    }
    void Land()
    {
        if(state != "landed")
        {
            timeCount = 0;
            rb.velocity = Vector3.zero;
            rest = Random.Range(0.25f, 1.25f);
            state = "landed";
        }
        if(gAnimation.state == "jumping")
        {
            gAnimation.state = "crouching";
        }
        if(gAnimation.state != "idle" && Wait(.25f))
        {
            gAnimation.state = "idle";
        }
        if(gAnimation.state == "idle" && Wait(1f))
        {
            state = "jumping";
            Jump();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            if(rightSideColliderCheck.isTouching) 
            {
                sign = -1;
            }
            else if(leftSideColliderCheck.isTouching)
            {
                sign = 1;
            }
            Land();
        }
        else if(collision.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(boing, 0.5f);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "BatCage")
        {
            Reverse();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gAnimation = GetComponent<GoopAnimation>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case "landed":
                Land();
                break;
            case "jumping":
                Jump();
                break;
        }
    }
    public override void Parry()
    {
        // Allows for the pacification of red goop.
        if(greenGoop != null)
        {
            Instantiate(greenGoop, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
