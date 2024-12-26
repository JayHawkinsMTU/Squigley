using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public followDaGuy pCamera;
    public GameObject eventSystem;
    public UncrouchCheck uncrouchCheck;
    public SideColliderCheck leftColliderCheck;
    public SideColliderCheck rightColliderCheck;

    public Vector3 checkPointCoords = new Vector3(0, 0, 0);
    public int checkPointLives = 0;
    Rigidbody2D playerRigidBody;
    SpriteRenderer spi;
    public float movespeed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 30f;
    public float deceleration = 50f;
    public float jumpForce = 650f;
    bool crouching = false;
    public bool grounded = false;
    public bool fastfallCharge = false;
    public bool dead = false;
    public bool inTightSpace = false;

    public AudioSource audioSource;
    public AudioClip jump;
    public AudioClip crouch;
    public AudioClip fastfall;
    public AudioClip death;
    public AudioClip land;
    public float volume=0.4f;
    //QOL
    private bool coyoteJump = false;
    private bool jumpBuffer = false;
    private bool jumpCooldown = false; //Is used to prevent double jump bugs. true if on cooldown.
    Wait deathWait = new Wait(0.25f); //The player has a quarter of a second of invincibility to prevent double-deaths.
    Wait unstretchWait = new Wait(1f);
    Wait coyoteWait = new Wait(0.1f);
    Wait bufferWait = new Wait(0.2f);
    Wait jumpCooldownWait = new Wait(0.1f); 
    //2P Integration
    public int playerID = 1;
    //Oblivion
    [SerializeField] bool oblivion = false;
    //New coin respawn.
    private List<Coin> coins;
    private List<Vase> vases;
    //physical movement
    public bool physicalMovement = true;
    private float innerVelX = 0;
    public float outerVelX = 0;
    private int flipped = 1; // Multiplier to certain movement when gravity is flipped.
    [SerializeField] GameObject[] fancyDeaths;
    [SerializeField] bool reloadOnDeath = false;
    
    // For latchboxes
    public Box boundTo;


    public void CoyoteDisable() //Used in uncrouchcheck to prevent levitating on a cieling.
    {
        coyoteJump = false;
    }
    //Fucking kills squigley
    public void Die(bool over = false) // Use override to skip over death animation. Used in fancy deaths to prevent recursion
    {
        if(!over && fancyDeaths.Length > 0)
        {
            // Fancy death should handle scene reloading
            Instantiate(fancyDeaths[Random.Range(0, fancyDeaths.Length)], transform.position, Quaternion.identity).GetComponent<FancyDeath>().StartAnimation(this);
            gameObject.SetActive(false);
        }
        else if(!dead) //Condition needed to create death cooldown -- PREVENTS DOUBLE TICKDOWN ON CHECKPOINT
        {
            if(boundTo != null) boundTo.Unbind();
            if(reloadOnDeath)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            gameObject.SetActive(true);
            audioSource.PlayOneShot(death, volume);
            dead = true;
            if(!oblivion)
            {
                transform.position = new Vector3(checkPointCoords.x, checkPointCoords.y, 0);
                if(checkPointLives > 0)
                {
                    checkPointLives--;
                    if(checkPointLives <= 0) {
                        Checkpoint.DeactivateAll();
                    }
                }
                ToPlayer1();
                RespawnCoins();
                eventSystem.GetComponent<UICoinHandler>().totalDeaths++;
            }
            else
            {
                eventSystem.GetComponent<ScoreHandler>().GameOver();
            }
        }
        
    }
    public void FlipGrav(int orientation)
    {
        if(orientation != 1 && orientation != -1)
        {
            Debug.Log("Wrong number in flipgrav, fuckass");
            return;
        }
        playerRigidBody.gravityScale = 2 * orientation;
        if(orientation == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        flipped = orientation;
    }
    public void ResetGrav()
    {
        playerRigidBody.gravityScale = 2;
        spi.flipY = false;
        flipped = 1;
    }
    private void RespawnCoins()
    {
        foreach(Coin c in coins)
        {
            c.Respawn();
        }
        foreach(Vase v in vases)
        {
            v.Respawn();
        }
    }
    void Jump()
    {
        if(!jumpCooldown && (grounded || coyoteJump || boundTo != null) )
        {
            if(boundTo != null) {
                boundTo.JumpOut();
            }
            jumpCooldown = true;
            coyoteJump = false;
            grounded = false;
            jumpBuffer = false;
            if(flipped == 1) transform.eulerAngles = new Vector3(0, 0, 0);
            else transform.eulerAngles = new Vector3(0, 0, 180);
            playerRigidBody.angularVelocity = 0;
            audioSource.PlayOneShot(jump, volume / 1.25f);
            //Only actually jumps if the player would benefit from the height. This prevents new players from fucking themselves over by spamming.
            if(jumpForce / 49f > playerRigidBody.velocity.y)
            {
                playerRigidBody.velocity = new Vector3(0f, 0f);
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce / 49f * flipped);
                jumpForce *= .5f;
            }
        }
    }
    void Crouch()
    {
        if(grounded && !crouching)
        {
            crouching = true;
            audioSource.PlayOneShot(crouch, volume);
            transform.localScale = new Vector3(1.2f, 0.5f);
            transform.Translate(0, -.24f, 0); //.24 is a more percise measure given the hitbox size, plus translate is more consistent.
        }
    }
    public void FastFall()
    {
        if(!grounded && fastfallCharge && !crouching)
        {
            if(boundTo != null) {
                boundTo.Unbind();
            }
            fastfallCharge = false;
            physicalMovement = false;
            playerRigidBody.velocity = new Vector3(0f, 0f);
            playerRigidBody.angularVelocity = 0;
            audioSource.PlayOneShot(fastfall, volume);
            transform.Translate(0, -.24f, 0); //Prevents sub-frame collision
            transform.localScale = new Vector3(0.5f, 1.2f);
            playerRigidBody.AddForce(transform.up * -1300f);
        }
    }
    public void Uncrouch()
    {
        if(crouching && !inTightSpace && uncrouchCheck.uncrouchCheck)
        {
            crouching = false;
            transform.localScale = new Vector3(1, 1);
            //transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f);
            transform.Translate(0, .24f, 0);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(physicalMovement)
        {
            outerVelX = 0f;
            playerRigidBody.velocity = new Vector2(outerVelX + innerVelX, playerRigidBody.velocity.y);
        }
        //Locks Ground movement speed
        if(movespeed > maxSpeed)
        {
            movespeed = maxSpeed;
        }
        if (collision.gameObject.tag == ("Environment"))
        {
            if(!fastfallCharge && transform.localScale.x != 1)
            {
                transform.Translate(0, -.24f, 0);
            }
            if(!inTightSpace && !crouching)
            {
                transform.localScale = new Vector3(1, 1);
            }
            
            if(!grounded)
            {
                if(flipped == 1) transform.eulerAngles = new Vector3(0, 0);
                else transform.eulerAngles = new Vector3(0, 0, 180);
                grounded = true;
                audioSource.PlayOneShot(land, .5f * volume);
                if(!crouching && GameInput.Crouch(playerID))
                {
                    /*if(UncrouchCheck.GetComponent<UncrouchCheck>().uncrouchCheck) IDK WHY THIS WAS HERE ORIGINALLY
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f);*/
                    crouching = true;
                    transform.localScale = new Vector3(1.2f,0.5f);
                    transform.Translate(0, -.28f, 0);
                }
            }
            
            //LedgePush(collision.gameObject);
            
            /*if(leftColliderCheck.isTouching || rightColliderCheck.isTouching) SUPPOSED PHASE SOLUTION, DIDNT WORK
            {
                if(transform.eulerAngles.z < 45 || transform.eulerAngles.z > 270)
                {
                    transform.eulerAngles = new Vector3(0,0,0);
                    playerRigidBody.angularVelocity = 0f;
                }
            }*/
            
            jumpForce = 650f;
            fastfallCharge = true;
        }
        if(collision.gameObject.tag == ("Anti-Jump"))
        {
            if(!inTightSpace && !crouching)
            {
                transform.localScale = new Vector3(1, 1);
            }
        }
        else if(collision.gameObject.tag == ("Hazards") || collision.gameObject.tag == "Player")
        {
            if(playerID == 1) Die();
            else GetComponent<SecondaryPlayerDeath>().Die();
        }
        /*else
        {
            grounded = false;
        }  THIS MIGHT BE IMPORTANT BUT IDK*/
        if(physicalMovement)
        {
            outerVelX = playerRigidBody.velocity.x - innerVelX;
            playerRigidBody.velocity = new Vector2(innerVelX + outerVelX, playerRigidBody.velocity.y);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        coyoteJump = true;
        grounded = false;
    }
    

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Environment"))
        {
            grounded = true;
            coyoteJump = false;
            fastfallCharge = true;
            if(!crouching && !fastfallCharge && !inTightSpace)
            {
                transform.localScale = new Vector3(1, 1);
            }
            
        }
        if(collision.gameObject.tag == ("Anti-Jump"))
        {
            if(!crouching && !fastfallCharge && !inTightSpace)
            {
                transform.localScale = new Vector3(1, 1);
            }
        }
    }
    

    void Accelerate(float maxSpeed)
    {
        if (movespeed <= maxSpeed)
        {
            movespeed = movespeed + acceleration * Time.deltaTime;
        }
    }
    void Decelerate()
    {
        if(movespeed >= 5)
        {
            movespeed = movespeed - deceleration * Time.deltaTime;
        }
    }
    void ToPlayer1()
    {
        if(playerID > 1)
        {
            GameObject player1 = GameObject.Find("Player");
            transform.position = new Vector3(player1.transform.position.x + 3f, player1.transform.position.y + 1f, 0f);
        }
    }
    private List<Coin> FindAllCoins()
    {
        IEnumerable<Coin> coinObjects = FindObjectsOfType<Coin>(true);

        return new List<Coin>(coinObjects);
    }
    private List<Vase> FindAllVases()
    {
        IEnumerable<Vase> vaseObjects = FindObjectsOfType<Vase>(true);

        return new List<Vase>(vaseObjects);
    }
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        spi=GetComponent<SpriteRenderer>();
        coins = FindAllCoins();
        vases = FindAllVases();
        eventSystem = GameObject.Find("EventSystem");
        ToPlayer1();
        Cursor.visible = false;
    }
    void Move(int dir)
    {
        if(boundTo != null) return;
        FFMove();
        bool canMove = false;
        float delta = movespeed * Time.deltaTime; //Change in x pos next frame.
        switch(dir)
        {
            case 1: //Right
                if(flipped == 1) spi.flipX = true;
                else spi.flipX = false;
                if(!rightColliderCheck.isTouching)
                    canMove = true;
                break;
            case -1: //Left
                if(flipped == 1) spi.flipX = false;
                else spi.flipX = true;
                if(!leftColliderCheck.isTouching)
                    canMove = true;
                break;
            default:
                Debug.LogError("Invalid direction");
                break;
        }
        if(Time.deltaTime > .1f) //Cannot move during stutter > 100ms
            canMove = false;
        /*RaycastHit2D aheadCast = CastAhead(delta, dir);
        if(!dead && aheadCast)
        {
            //Debug.Log(aheadCast.collider.gameObject);
            canMove = false;
            Debug.Log("castahead");
            Flush(aheadCast.collider.gameObject);
        }*/
        //playerRigidBody.angularVelocity = 0f;
        if(grounded)
        {
            if(flipped == 1) transform.eulerAngles = Vector3.zero;
            else transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if(canMove)
        {
            Accelerate(maxSpeed);
            //playerRigidBody.MovePosition((Vector2)transform.position + (playerRigidBody.velocity * Time.deltaTime));
            //Physics.SyncTransforms();
            if(physicalMovement)
            {
                innerVelX = movespeed * dir;
                
            }
            else
            {
                transform.position = new Vector3(transform.position.x + delta * dir, transform.position.y);
            }
        }
        else
        {
            if(physicalMovement)
            {
                innerVelX = 0;
            }
            //Decelerate();
        }
    }
    RaycastHit2D CastAhead(float deltaX, int dir)
    {
        return Physics2D.BoxCast(transform.position, transform.localScale * .9f, 0, Vector3.right * dir, Mathf.Abs(deltaX));
    }
    void Flush(GameObject g, float modifier = 0f) //Sets players x coordinate to be flush with gameobject g
    {
        if(g.transform.rotation.z == 0)
        {
            if(transform.position.x < g.transform.position.x)
            {
                transform.position = new Vector3(g.transform.position.x - g.transform.localScale.x / 2 - .475f - modifier, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(g.transform.position.x + g.transform.localScale.x / 2 + .475f + modifier, transform.position.y, transform.position.z);
            }
        }
        else
        {
            Debug.Log(g + " is a platform with a nonzero rotation.");
        }
    }
    public void ResetVels()
    {
        innerVelX = 0;
        outerVelX = 0;
    }

    bool IsBetween(float x, float min, float max)
    {
        return x >= min && x <= max;
    }
    private bool NotFallingSideways()
    {
        return Mathf.Abs(playerRigidBody.velocity.x) < maxSpeed + 5 || (IsBetween(transform.eulerAngles.z, -5, 5) || IsBetween(transform.eulerAngles.z, 175, 185));
    }
    // This function is used on move in order to prevent fastfall halting.
    private void FFMove()
    {
        if(fastfallCharge)
        {
            physicalMovement = true;
        }
        /*else
        {
            outerVelX = 0;
        }*/
    }

    private void HandleInput()
    {
        if(GameInput.FastFall(playerID))
        {
            FastFall();
            // On fastfall, you can't do anything else on the same frame.
            // This is to prevent bugs where player movement halts.
            return;
        }
        //INPUT
        if (GameInput.MoveRight(playerID) && !rightColliderCheck.isTouching && NotFallingSideways())
        {
            Move(1 * flipped);
        }
        else if(GameInput.MoveLeft(playerID) && !leftColliderCheck.isTouching && NotFallingSideways())
        {
            Move(-1 * flipped);
        }
        else
        {
            if(physicalMovement)
            {
                innerVelX = 0;
                playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            }
            physicalMovement = false;
        }
        if(!GameInput.MoveLeft(playerID) && !GameInput.MoveRight(playerID))
        {
            Decelerate();
        }
        if(GameInput.Jump(playerID))
        {
            jumpBuffer = true;
            Jump();
        }
        if(GameInput.Crouch(playerID))
        {
            Crouch();
        }
        else if(crouching && uncrouchCheck.uncrouchCheck)
        {
            Uncrouch();
        }
        
    }
    void Update()
    {
        if(eventSystem == null || !eventSystem.GetComponent<Pause>().paused)
        {
            if(!fastfallCharge)
            {
                unstretchWait.Iterate();
                if(unstretchWait.isDone())
                {
                    if(!inTightSpace && !grounded && Mathf.Abs(playerRigidBody.velocity.y) < 4f)
                    {
                        transform.localScale = new Vector3(1,1,1);
                        unstretchWait.Reset();
                    }
                }
            }
            else
            {
                unstretchWait.Reset();
            }
            

            //TIME EVENTS
            if(dead)
            {
                deathWait.Iterate();
                if(deathWait.Complete())
                {
                    dead = false;
                }
            }
            if(jumpCooldown)
            {
                jumpCooldownWait.Iterate();
                if(jumpCooldownWait.Complete())
                {
                    jumpCooldown = false;
                }
            }
            if(jumpBuffer) //Allows for the player to jump as soon as it becomes available in a certain time frame.
            {
                coyoteJump = false;
                Jump();
                bufferWait.Iterate();
                if(bufferWait.Complete())
                {
                    jumpBuffer = false;
                }
            }
            else
            {
                bufferWait.Reset();
            }
            if(coyoteJump) //Coyote jump allows for the player to jump shortly after leaving a platform.
            {
                coyoteWait.Iterate();
                if(coyoteWait.Complete())
                {
                    coyoteJump = false;
                }
            }
            else
            {
                coyoteWait.Reset();
            }
            
            if(grounded)
            {
                playerRigidBody.freezeRotation = true;
            }
            else
            {
                playerRigidBody.freezeRotation = false;
            }
            
            if(Mathf.Abs(playerRigidBody.velocity.x) < .05f)
            {
                outerVelX = 0;
                innerVelX = 0;
            } 
            
            HandleInput();

            /*if(crouching) //Locks rotation if crouched.
            {
                transform.eulerAngles = new Vector3(0,0,0);
                playerRigidBody.angularVelocity = 0f;
            }*/

            
            //Physical movement handling END
            if(physicalMovement)
            {
                playerRigidBody.velocity = new Vector2(innerVelX + outerVelX, playerRigidBody.velocity.y);
                //In most cases, outerVelX will be 0 or close to 0. Big exceptions would be fastfalling sideways.
                outerVelX = playerRigidBody.velocity.x - innerVelX;
            }
            

            if(pCamera != null && !pCamera.smoothCamera) //Camera should only be null if it's player 2.
                pCamera.UpdateCamera();

            //yVel = playerRigidBody.velocity.y; //updates y velocity
            

            /*Player2 Death sync
            if(playerID > 1)
            {
                GameObject player1 = GameObject.Find("Player");
                if(player1.GetComponent<Movement>().dead && !dead)
                {
                    transform.position = new Vector3(player1.transform.position.x + 3f, player1.transform.position.y + 1f, 0f);
                }
            }*/
        }
    }
    
}
    
