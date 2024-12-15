using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedGround : MonoBehaviour
{
    GameObject player;
    public GameObject eventSystem;
    public Collider2D boxCollider;
    public AudioSource audioSource;
    public AudioClip breakSound;
    public AudioClip respawnSound;
    public GameObject sprite;
    float timeCount = 0;
    bool active = false;
    private bool dead = false;
    bool soundPlayed = false;
    float opacity = 0;

    MultiplayerHandler multiplayer;
    GameObject player1;
    GameObject player2;
    void BootPlayer()
    {
        player = NearestPlayer();
        if(Physics2D.Raycast(transform.position, Vector2.up, transform.localScale.y / 2))
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
    void TimeCrumble(float seconds)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= seconds)
        {
            UICoinHandler hand = eventSystem.GetComponent<UICoinHandler>();
            if(hand != null) hand.blocksBroken++;
            audioSource.PlayOneShot(breakSound, 0.5f);
            GetComponent<ParticleSystem>().Play();
            boxCollider.enabled = false;
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
            timeCount = 0;
            dead = true;
            active = false;
        }
    }
    void Respawn(float respawnTime)
    {
        timeCount += Time.deltaTime;
        if(timeCount >= respawnTime - 1 && timeCount < respawnTime)
        {
            if(PlayerInside()) BootPlayer();
            if(!soundPlayed)
            {
                sprite.transform.localPosition = new Vector3(0, 0, 0);
                audioSource.PlayOneShot(respawnSound, 0.5f);
                soundPlayed = true;
            }
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,opacity);
            opacity += Time.deltaTime;
            if(opacity > 1)
            {
                opacity = 1;
            }
        }
        else if(timeCount >= respawnTime && !PlayerInside())
        {
            //BootPlayer();
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
            
            opacity = 0;
            boxCollider.enabled = true;
            soundPlayed = false;
            timeCount = 0;
            dead = false;
        }
    }
    bool PlayerInside()
    {
        player = NearestPlayer();
        return Mathf.Abs(player.transform.position.y - transform.position.y) <= transform.localScale.y / 2 && Mathf.Abs(player.transform.position.x - transform.position.x) <= transform.localScale.x / 2;
    }
    void Shake(GameObject sprite)
    {
        sprite.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f),Random.Range(-0.1f, 0.1f), 0);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            active = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
        player = GameObject.Find("Player");
        player1 = player;
        multiplayer = eventSystem.GetComponent<MultiplayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            TimeCrumble(0.5f);
            Shake(sprite);
        }
        if(dead)
        {
            Respawn(5);
        }
    }
}
