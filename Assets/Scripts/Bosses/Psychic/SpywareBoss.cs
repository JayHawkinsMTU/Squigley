using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpywareBoss : MonoBehaviour
{
    const int MAX_HEALTH = 10;
    const float HEAD_HEIGHT = 0.125f; //The height above the center line thats counted as the head hitbox.
    const float HIT_JUMP_VEL = 5;
    const float INVINC_LENGTH = 3;
    const float JUMP_BOUNDS = 4; //Difference in y required for jump upon chase.
    const float HB_GRACE = 0.45f; //Time after start before hitbox appears. 
    const float HB_LENGTH = 0.1f; //Time that the hitbox stays out for.
    const float SLICE_COOLDOWN = 1.25f;
    const float SHOOT_GRACE = 0.4f; //Time between telegraphing shoot and bullet appearing.
    const float SAFETY_DIST = 1.25f; //Distance where bullet appears relative to spyware.
    const float BULLET_VELOCITY = 13.75f;
    const float POST_SHOOT_WAIT = 0.75f; //The time spyware waits after shooting to chase.
    const float STUN_LENGTH = 1f;
    const float TELE_DISTANCE = 12.5f; //Distance required to teleport
    const float TELE_GRACE = 1.25f; //Time it takes from marker appearing until teleport.
    const float TELE_COOLDOWN = 6;
    const float SHOOT_BOUND_X = 5.75f; //Difference in x values required to shoot.
    const float SHOOT_BOUND_Y = 5f; //Must be inside these boundaries to shoot.
    const float STARTING_SHOOT_COOLDOWN = 5; //Seconds it takes to "reload"
    const float EXIT_BOUNDS = 55f; //Distance where the player is considered to have left. (Reset)
    const int BLINKS = 25; //# of times sprite blinks before disabling invincibility
    public int hp = MAX_HEALTH;
    //public bool grounded;
    private bool invincible = false;
    private bool jumpAnim = true;
    private bool runAnim = false;
    private bool shooting = false;
    private bool slicing = false;
    private bool tping = false;
    private float shootCooldown = STARTING_SHOOT_COOLDOWN;
    Transform player;
    Rigidbody2D rb;
    [SerializeField] int runFps = 8;
    //Visuals
    SpriteRenderer spi;
    [SerializeField] Sprite[] runAnimation;
    [SerializeField] Sprite[] jumpAnimation;
    [SerializeField] Sprite[] sliceAnimation;
    [SerializeField] Sprite shootingAnimation;
    [SerializeField] Sprite stand;
    [SerializeField] Sprite crouch;
    [SerializeField] SideColliderCheck groundCheck;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject warpMarker;
    [SerializeField] GameObject deadSpyware;
    [SerializeField] float jumpVel = 18.5f;
    [SerializeField] float runVel = 6f;
    [SerializeField] BossHealthBar healthBar;
    
    private Vector3 startingPos;
    private Vector3 initHbOffset;
    private GameObject marker;
    //Audio
    AudioSource audioSource;
    public AudioClip jumpSound, slashSound, shootSound, teleportSound, alert, headShot, grunt;

    void Reset()
    {
        hp = MAX_HEALTH;
        transform.position = startingPos;
        healthBar.Disable();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        healthBar.Attach("SPYWARE", MAX_HEALTH);
        healthBar.Enable();
        tping = true;
    }

    int PlayerDirection()
    {
        //To the right of spyware
        if(player.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            return 1;
        } 
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            return -1;
        } 
    }
    int SelfDirection()
    {
        if(transform.eulerAngles.y == 0) return 1;
        else return -1;
    }

    void TickHealth(int delta)
    {
        if(invincible) return;
        StopAllCoroutines();
        runAnim = false;
        slicing = false;
        tping = false;
        shooting = false;
        if(marker != null) Destroy(marker);
        hp -= delta;
        healthBar.UpdateBar(hp);
        rb.velocity = new Vector2(PlayerDirection() * HIT_JUMP_VEL, HIT_JUMP_VEL);
        if(hp <= 8)
        {
            runVel = 7f;
            jumpVel = 19f;
            if(hp <= 5)
            {
                jumpVel = 19.5f;
                runVel = 8f;
                shootCooldown = 4;
            }
            if(hp <= 3)
            {
                jumpVel = 20f;
                runVel = 8.5f;
                shootCooldown = 2;
            }
        }
        if(hp <= 0)
        {
            Die();
        }
        StartCoroutine(IFrames());
    }

    void Teleport()
    {
        if(!tping) StartCoroutine(Warp());

        IEnumerator Warp()
        {
            runAnim = false;
            jumpAnim = false;
            shooting = false;
            tping = true;
            while(!groundCheck.isTouching)
            {
                yield return new WaitForEndOfFrame();
            }
            spi.sprite = crouch;
            rb.velocity = Vector2.zero;
            marker = Instantiate(warpMarker, player.position, Quaternion.identity) as GameObject;
            yield return new WaitForSeconds(TELE_GRACE);
            audioSource.PlayOneShot(teleportSound);
            transform.position = marker.transform.position;
            transform.position += new Vector3(0, 1, 0); //Moves up slightly to correct for difference in size
            jumpAnim = true;

            yield return new WaitForSeconds(.75f);
            Destroy(marker);
            yield return new WaitForSeconds(TELE_COOLDOWN - 2);
            tping = false;
        }
    }
    public void TurnAround(int dir)
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        rb.velocity = new Vector2(runVel * dir, rb.velocity.y);
    }
    public void Chase()
    {
        if(!groundCheck.isTouching) return;
        //Either turn around or jump or neither, unless both checked
        if(player.position.y - transform.position.y < -2) 
        {
            rb.velocity = new Vector2(runVel * SelfDirection(), rb.velocity.y);
            return; //Don't jump or turn around if player is below.
        }
        int pDir = PlayerDirection();
        if(pDir == SelfDirection() || rb.velocity.x == 0)
        {
            if(!runAnim) StartCoroutine(RunAnim());
            rb.velocity = new Vector2(runVel * pDir, rb.velocity.y);
        }
    }

    public void Jump(float jumpBound, float jumpMultiplier = 0)
    {
        if(!groundCheck.isTouching || jumpBound >= 100 || player.position.y < transform.position.y + jumpBound ) return;
        runAnim = false;
        jumpAnim = true;
        audioSource.PlayOneShot(jumpSound, 0.55f);
        rb.velocity = new Vector2(rb.velocity.x, jumpVel * jumpMultiplier);
        audioSource.PlayOneShot(jumpSound, 0.5f);
    }

    void MovementAnimation()
    {
        //Jumping and falling
        if(!groundCheck.isTouching && jumpAnim)
        {
            if(rb.velocity.y > 2)
            {
                spi.sprite = jumpAnimation[0];
            }
            else if(rb.velocity.y > -2)
            {
                spi.sprite = jumpAnimation[1];
            }
            else
            {
                spi.sprite = jumpAnimation[2];
            }
        }
    }

    public void ActivateSlice()
    {
        if(groundCheck.isTouching && !slicing && !invincible) StartCoroutine(Slice());
        IEnumerator Slice()
        {
            slicing = true;
            runAnim = false;
            yield return new WaitForSeconds(0.025f);
            rb.velocity = Vector2.zero;
            spi.sprite = sliceAnimation[0]; //Starts first frame
            audioSource.PlayOneShot(alert, 0.65f);
            yield return new WaitForSeconds((float) HB_GRACE * (5f/6f));
            int index = sliceAnimation.Length - 1;
            spi.sprite = sliceAnimation[index];
            yield return new WaitForSeconds((float) HB_GRACE / 6f);
            hitbox.transform.localPosition = initHbOffset;
            hitbox.transform.parent = null; //Detaches hitbox
            hitbox.SetActive(true); //Activates hitbox
            audioSource.PlayOneShot(slashSound, 0.65f);
            yield return new WaitForSeconds(HB_LENGTH);
            hitbox.SetActive(false); //Disables hitbox
            hitbox.transform.parent = transform; //Reattaches hitbox
            while(index > 0) //Lowering blade animation
            {
                yield return new WaitForSeconds(0.125f);
                spi.sprite = sliceAnimation[--index];
            }
            Chase();

            //Cooldown handling
            yield return new WaitForSeconds(SLICE_COOLDOWN);
            slicing = false;
        }
    }

    

    IEnumerator RunAnim()
    {
        runAnim = true;
        int index = 0;
        while(groundCheck.isTouching && runAnim) //Run animation can be stopped from outside.
        {
            yield return new WaitForSeconds(1f / (float) runFps);
            if(!runAnim) break;
            spi.sprite = runAnimation[index++ % runAnimation.Length]; //Loops through animation
        }
        runAnim = false;
    }

    IEnumerator IFrames()
    {
        invincible = true;
        for(int i = 0; i < BLINKS; i++)
        {
            yield return new WaitForSeconds(INVINC_LENGTH / BLINKS);
            if(spi.color.a <= 0.25f)
            {
                spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, 1);
            }
            else
            {
                spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, 0.25f);
            }
        }
        invincible = false;
        spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, 1);
        Chase();
    }

    public void ActivateShoot()
    {
        if(!groundCheck.isTouching || shooting || slicing || tping) return; //Don't shoot if grounded or already shooting
        PlayerDirection(); //Turns to player
        StartCoroutine(Shoot());

        IEnumerator Shoot() //This should not be called without ground check, thus nest.
        {
            shooting = true;
            runAnim = false;
            jumpAnim = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            spi.sprite = shootingAnimation;
            yield return new WaitForSeconds(SHOOT_GRACE);
            spi.sprite = shootingAnimation;
            int orientation = SelfDirection();
            audioSource.PlayOneShot(shootSound, 0.55f);
            Instantiate(projectile, new Vector3(transform.position.x + orientation * SAFETY_DIST, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Projectile>().Fire(orientation * BULLET_VELOCITY, 0);
            audioSource.PlayOneShot(shootSound, 0.75f);
            yield return new WaitForSeconds(POST_SHOOT_WAIT);
            runAnim =false; //Confirms that chase function starts run animation.
            Chase(); //Don't jump

            //Cooldown handling
            yield return new WaitForSeconds(shootCooldown);
            shooting = false;
            jumpAnim = true;
        }

    }

    public void ActivateStun()
    {
        StartCoroutine(Stun());
        runAnim = false;
        IEnumerator Stun()
        {
            rb.velocity = new Vector2(0, rb.velocity.y); //Stops x movement
            while(!groundCheck.isTouching) {
                yield return new WaitForEndOfFrame();
            } //Waits until grounded
            jumpAnim = false;
            runAnim = false;
            spi.sprite = stand;
            yield return new WaitForSeconds(STUN_LENGTH);
            if(!slicing) jumpAnim = true;
            Chase();
        }
    }
    

    

    void Die()
    {
        shooting = true;
        slicing = true;
        tping = true;
        runAnim = false;
        rb.simulated = false;
        healthBar.Disable();
        StopAllCoroutines();
        StartCoroutine(Death());
        
        IEnumerator Death()
        {
            while(Vector2.Distance(transform.position, deadSpyware.transform.position) > 0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, deadSpyware.transform.position, Vector2.Distance(transform.position, deadSpyware.transform.position) * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            deadSpyware.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        rb.velocity = Vector2.zero;
        if(other.gameObject.CompareTag("Environment"))
        {
            if(groundCheck.isTouching) Chase();
            else Chase();
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            ActivateStun();
        }
        else if(other.gameObject.CompareTag("Hazards"))
        {
            int delta = 1;
            audioSource.PlayOneShot(grunt, 0.75f);
            if(other.transform.position.y > transform.position.y + HEAD_HEIGHT)
            {
                delta = 2;
                audioSource.PlayOneShot(headShot);
            } 
            TickHealth(delta);
        }
    }

    void OnEnable()
    {
        Activate();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        spi = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        startingPos = transform.position;
        initHbOffset = hitbox.transform.localPosition;
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
        if(groundCheck.isTouching)
        {
            if(Mathf.Abs(player.position.x - transform.position.x) > SHOOT_BOUND_X && Mathf.Abs(player.position.y - transform.position.y) < SHOOT_BOUND_Y)
            {
                ActivateShoot();
            }
        }
        float dist = Vector2.Distance(player.position, transform.position);
        if(dist >= EXIT_BOUNDS)
        {
            Reset();
        }
        else if(dist >= TELE_DISTANCE)
        {
            Teleport();
        }
    }
}
