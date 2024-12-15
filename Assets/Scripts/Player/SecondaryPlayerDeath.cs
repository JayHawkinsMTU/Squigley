using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryPlayerDeath : MonoBehaviour
{
    bool dead = false;
    bool respawning = false;
    [SerializeField] float respawnTime = 1.5f;

    float timeCount = 0;
    Vector2 respawnCoords;
    Movement player1;
    Movement movement;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip respawnSound;
    [SerializeField] AudioClip dieSound;
    SpriteRenderer spi;
    Collider2D boxCollider;
    Rigidbody2D playerRigidbody;
    float opacity = 0f;

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
    public void Die()
    {
        dead = true;
        spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, 0f);
        boxCollider.enabled = false;
        playerRigidbody.velocity = new Vector3(0f, 0f, 0f);
        playerRigidbody.isKinematic = true;
        transform.localScale = new Vector2(1,1);
        movement.enabled = false;
        audioSource.PlayOneShot(dieSound, 0.3f);
    }
    void FadeIn()
    {
        if(!respawning)
        {
            audioSource.PlayOneShot(respawnSound, 0.7f);
            if(player1.dead)
            {
                respawnCoords = new Vector2(player1.transform.position.x + 3, player1.transform.position.y + 2); //Spawns P2 further if P1 is dead
            }
            else
            {
                respawnCoords = new Vector2(player1.transform.position.x + 2, player1.transform.position.y);
            }
            transform.position = respawnCoords;
        }
        respawning = true;
        if(opacity < 1f)
        {
            spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, opacity);
            opacity += (Time.deltaTime / respawnTime);
        }
        if(Wait(respawnTime))
        {
            dead = false;
            respawning = false;
            spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, 1f);
            opacity = 0f;
            boxCollider.enabled = true;
            playerRigidbody.isKinematic = false;
            playerRigidbody.velocity = new Vector2(0, 4f);
            movement.ResetVels();
            movement.enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player").GetComponent<Movement>();
        boxCollider = GetComponent<Collider2D>();
        spi = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
        {
            FadeIn();
        }
    }
}
