using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilroyMovement : MonoBehaviour
{
    [SerializeField] SideColliderCheck lColliderCheck;
    [SerializeField] SideColliderCheck rColliderCheck;
    BossActivate bossActivate;
    Rigidbody2D rb = null;
    KilroyVulnerability vState;
    SpriteRenderer spi;
    AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crouchSound;
    [SerializeField] AudioClip fastFallSound;

    bool grounded = true;
    bool crouched = false;
    bool ffCharge = false;
    string phase = "SWEEP";
    /*
    SWEEP - MOVE BACK AND FOURTH WHILE GROUNDED
    SPASM - JUMP AND FASTFALL WHILE MOVING BACK AND FORTH
    JSWEEP - MOVE BACK AND FORTH AND JUMP
    STUN - STAND STILL FOR A FEW SECONDS, ONLY AT MAX LIVES
    CORNERCOWER - WALLJUMP TO CORNER
    CROUCHCOWER - SWEEP WHILE CROUCHING
    RFF - GO TO SIDE > JUMP > ROTATE > FASTFALL
    DEAD - DEAD
    */
    int direction = -1;
    /*
    -1 left
    0 still
    1 right
    */
    bool reverseOnWall = true;
    bool jumpOnWall = false;
    bool wallTag = false;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 650f;
    [SerializeField] float ffForce = 1300f;

    Wait phaseWait;
    Wait inputWait;
    int inputCounter = 0;
    int toCenter() //Returns the direction corresponding to the center.
    {
        if(transform.position.x < bossActivate.initPosition.x) return 1;
        else return -1;
    }
    void Move(float direction)
    {
        if(direction == -1 && !lColliderCheck.isTouching)
        {
            spi.flipX = false;
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
        else if(direction == 1 && !rColliderCheck.isTouching)
        {
            spi.flipX = true;
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        if(reverseOnWall && (lColliderCheck.isTouching || rColliderCheck.isTouching))
        {
            direction *= -1;
        }
    }
    
    bool FastFall()
    {
        if(!grounded && ffCharge)
        {
            ffCharge = false;
            transform.localScale = new Vector3(0.5f, 1.2f);
            rb.AddForce(transform.up * -ffForce);
            audioSource.PlayOneShot(fastFallSound, 0.25f);
            return true;
        }
        return false;
    }
    bool Crouch()
    {
        if(grounded && !crouched)
        {
            //Debug.Log("Crouched");
            crouched = true;
            transform.localScale = new Vector3(1.2f, 0.5f);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.24f);
            audioSource.PlayOneShot(crouchSound, 0.25f);
            return true;
        }
        return false;
    }
    void Uncrouch()
    {
        if(crouched)
        {
            crouched = false;
            transform.localScale = new Vector3(1f, 1f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.24f);
        }
    }
    bool Jump()
    {
        if(grounded)
        {
            grounded = false;
            ffCharge = true;
            rb.AddForce(transform.up * jumpForce);
            audioSource.PlayOneShot(jumpSound, 0.25f);
            return true;
        }
        return false;
    }
    void Rotate()
    {
        if(transform.position.x < bossActivate.initPosition.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }
    
    // Start is called before the first frame update
    public void Start()
    {
        direction = -1;
        vState = GetComponent<KilroyVulnerability>();
        spi = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        bossActivate = GetComponent<BossActivate>();
        phaseWait = new Wait(8f);
        inputWait = new Wait(1f);
        ChangePhases("SWEEP");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            if(!crouched)
                transform.localScale = new Vector3(1f, 1f, 1f);
            transform.eulerAngles = new Vector3(0, 0, 0);
            if(rb != null)
                rb.velocity = new Vector3(0, 0, 0);
            if(lColliderCheck.isTouching || rColliderCheck.isTouching)
            {
                //Debug.Log("WALL TOUCH");
                if(reverseOnWall) direction *= -1;
                if(jumpOnWall && !wallTag) Jump();
                wallTag = true;
            }
            else
            {
                wallTag = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            if(!crouched)
                transform.localScale = new Vector3(1f, 1f, 1f);
            transform.eulerAngles = new Vector3(0, 0, 0);
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
    void ChangePhases(string p)
    {
        //Debug.Log("FROM: " + phase + " TO: " + p);
        phase = p;
        Uncrouch();
        inputCounter = 0;
        phaseWait.Reset();
        inputWait.Reset();
        switch(phase)
        {
            case "SWEEP":
                phaseWait = new Wait(6f);
                reverseOnWall = true;
                jumpOnWall = false;
                break;
            case "JSWEEP":
                phaseWait = new Wait(6f);
                inputWait = new Wait(1f);
                reverseOnWall = true;
                jumpOnWall = true;
                break;
            case "SPASM":
                phaseWait = new Wait(6f);
                inputWait = new Wait(0.25f);
                reverseOnWall = true;
                jumpOnWall = false;
                break;
            case "RFF":
                reverseOnWall = true;
                jumpOnWall = true;
                inputWait = new Wait(3f);
                phaseWait = new Wait(12f);
                break;
            case "CROUCHCOWER":
                phaseWait = new Wait(5f);
                inputWait = new Wait(.25f);
                reverseOnWall = true;
                jumpOnWall = false;
                break;
            case "CORNERCOWER":
                jumpOnWall = true;
                reverseOnWall = true;
                inputWait = new Wait(0.5f);
                phaseWait = new Wait(5f);
                break;
            case "STUN":
                phaseWait = new Wait(5f);
                break;
        }
    }

    void VulnerableOverride( )
    {
        if(vState.state == "VULNERABLE" && phase != "STUN" && phase != "CROUCHCOWER" && phase != "CORNERCOWER" && phase != "DEAD")
        {
            if(vState.lives == 5)
            {
                ChangePhases("STUN");
            }
            else if(vState.lives == 4 || vState.lives == 3)
            {
                ChangePhases("CROUCHCOWER");
            }
            else if(vState.lives > 0)
            {
                phase = (Random.Range(0, 2) > 1) ? "CROUCHCOWER" : "CORNERCOWER"; 
                ChangePhases(phase);
            }
            else
            {
                ChangePhases("DEAD");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        VulnerableOverride();
        phaseWait.Iterate();
        switch(phase)
        {
            case "SWEEP":
                Move(direction);
                if(phaseWait.Complete())
                {
                    if(vState.lives == 5) ChangePhases("JSWEEP");
                    else ChangePhases("SPASM");
                }
                break;
            case "JSWEEP":
                Move(direction);
                inputWait.Iterate();
                if(grounded && inputWait.Complete()) Jump();
                if(phaseWait.Complete())
                {
                    phase = (Random.Range(0,2) > 1) ? "SWEEP" : "SPASM";
                    ChangePhases(phase);
                }
                break;
            case "SPASM":
                Move(direction);
                inputWait.Iterate();
                if(inputWait.Complete())
                {
                    if(grounded && !lColliderCheck.isTouching && !rColliderCheck.isTouching) 
                    {
                        Jump();
                    } else FastFall();
                } 
                if(phaseWait.Complete())
                {
                    FastFall();
                    ChangePhases("RFF");
                }
                break;
            case "RFF":
                if(transform.position.x > bossActivate.initPosition.x - 6 && transform.position.x < bossActivate.initPosition.x + 6)
                {
                    inputWait = new Wait(0.25f);
                    Move(direction);
                    inputCounter = 0;
                }
                else if(transform.position.x < bossActivate.initPosition.x - 6.5 || transform.position.x > bossActivate.initPosition.x + 6.5)
                {
                    inputWait = new Wait(0.25f);
                    Move(toCenter());
                    inputCounter = 0;
                }
                else
                {
                    inputWait.Iterate();
                    if(inputWait.isDone())
                    {
                        if(inputCounter % 3 == 0)
                        {
                            if(Jump()) //Only proceeds if jump is successful.
                            {
                                Jump();
                                inputCounter++;
                                inputWait = new Wait(Random.Range(0.45f, 0.65f));
                            }
                        }
                        else if(inputCounter % 3 == 1)
                        {
                            Rotate();
                            inputCounter++;
                            inputWait = new Wait(0.25f);
                        }
                        else if(inputCounter % 3 == 2)
                        {
                            if(FastFall())
                            {
                                direction *= -1;
                                inputCounter++;
                                inputWait = new Wait(0.5f);
                            }
                            
                        }
                    }
                    
                }
                if(phaseWait.Complete())
                {
                    ChangePhases("JSWEEP");
                }
                break;
            case "CROUCHCOWER":
                inputWait.Iterate();
                if(inputWait.isDone())
                {
                    if(inputCounter % 2 == 0)
                    {
                        if(Crouch())
                        {
                            inputCounter++;
                            inputWait.Reset();
                        }
                    }
                    else
                    {
                        Uncrouch();
                        inputCounter++;
                        inputWait.Reset();
                    }
                }
                Move(direction);
                if(phaseWait.Complete())
                {
                    if(vState.lives == 5) ChangePhases("JSWEEP");
                    else ChangePhases("SPASM");
                }
                break;
            case "CORNERCOWER":
                Move(direction * .75f);
                if(wallTag)
                {
                    Jump();
                    inputWait.Iterate();
                    if(inputWait.Complete())
                    {
                        wallTag = false;
                        direction *= -1;
                    }
                }
                if(phaseWait.Complete())
                {
                    ChangePhases("JSWEEP");
                }
                break;
            case "STUN":
                if(phaseWait.Complete())
                {
                    ChangePhases("SWEEP");
                }
                break;
        }

    }
}
