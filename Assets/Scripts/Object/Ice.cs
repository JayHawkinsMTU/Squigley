using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject player;
    public Collider2D boxCollider;
    public SpriteRenderer sprite;
    public AudioSource audioSource;
    public AudioClip shatter;
    public AudioClip respawnSound;
    bool soundPlayed;
    float timeCount = 0;
    float opacity = 0f;
    public float respawnTime = 5;
    float playerVelocity;
    bool slippery = false;

    bool broken = false;
    float playerMovespeed;
    float playerAcceleration;
    float playerDeceleration;

    MultiplayerHandler multiplayer;
    GameObject player1;
    GameObject player2;
    void BootPlayer()
    {
        player = NearestPlayer();
        if(Physics2D.Raycast(transform.position, Vector2.up, transform.localScale.y / 2)) //Only boots player if there is space available above.
        {
            //Debug.Log("Booted");
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + transform.localScale.y, player.transform.position.z);
            player.GetComponent<Movement>().Uncrouch();
        }
    }
    GameObject NearestPlayer()
    {
        if(multiplayer.dropIn)
        {
            player2 = GameObject.Find("Player 2");
            if(Vector2.Distance(transform.position, player1.transform.position) > Vector2.Distance(transform.position, player2.transform.position))
            {
                return player2;
            }
            else
            {
                return player1;
            }
        }
        else if(player == null)
        {
            return GameObject.Find("Player");
        }
        return player;
    }
    void SetPlayerValues()
    {
        if(!slippery)
        {
            slippery = true;
            player.GetComponent<Movement>().maxSpeed = 25;
            player.GetComponent<Movement>().acceleration = playerAcceleration * 1/1.5f;
            player.GetComponent<Movement>().deceleration = playerDeceleration * 1/4;
        }
    }
    void ResetPlayerValues()
    {
        if(slippery)
        {
            slippery = false;
            player.GetComponent<Movement>().maxSpeed = playerMovespeed;
            player.GetComponent<Movement>().acceleration = playerAcceleration;
            player.GetComponent<Movement>().deceleration = playerDeceleration;

        }
    }
    void Break()
    {
        ResetPlayerValues();
        audioSource.PlayOneShot(shatter, 0.5f);
        GetComponent<ParticleSystem>().Play();
        boxCollider.enabled = false;
        sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
        UICoinHandler hand = eventSystem.GetComponent<UICoinHandler>();
        if(hand != null) eventSystem.GetComponent<UICoinHandler>().blocksBroken++;
        timeCount = 0;
        broken = true;
    }
    void Respawn()
    {
        timeCount += Time.deltaTime;
        if(timeCount >= respawnTime - 1 && timeCount < respawnTime)
        {
            if(PlayerInside()) BootPlayer();
            if(!soundPlayed)
            {
                audioSource.PlayOneShot(respawnSound, 0.5f);
                soundPlayed = true;
            }
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,opacity);
            opacity += (Time.deltaTime / 2);
            if(opacity > 0.5)
            {
                opacity = 0.5f;
            }
        }
        else if(timeCount >= respawnTime && !PlayerInside())
        {
            //BootPlayer();
            //Debug.Log("Respawned");
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
            
            opacity = 0;
            boxCollider.enabled = true;
            soundPlayed = false;
            timeCount = 0;
            broken = false;
        }
    }
    bool PlayerInside()
    {
        return Mathf.Abs(player.transform.position.y - transform.position.y) <= transform.localScale.y / 2 && Mathf.Abs(player.transform.position.x - transform.position.x) <= transform.localScale.x / 2;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            //Debug.Log(playerVelocity);
            if(playerVelocity > 15)
            {
                Break();
            }
            else
            {
                SetPlayerValues();
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        ResetPlayerValues();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player1 = player;
        playerMovespeed = player.GetComponent<Movement>().maxSpeed;
        playerAcceleration = player.GetComponent<Movement>().acceleration;
        playerDeceleration = player.GetComponent<Movement>().deceleration;
        eventSystem = GameObject.Find("EventSystem");
        multiplayer = eventSystem.GetComponent<MultiplayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        player = NearestPlayer();
        //playerVelocity is calculated here to get it the frame before collision instead of after -- where it would be effectively 0.
        playerVelocity = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y);
        if(broken)
        {
            Respawn();
        }
    }
}
