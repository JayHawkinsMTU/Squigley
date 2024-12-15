using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCoin : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinSound;
    public Collider2D boxCollider;
    public SpriteRenderer spi;
    public GameObject player;
    bool collected = false;
    float timeCount = 0f;
    [SerializeField] float respawnTime = 5f;
    float opacity = 0f;
    void Collect()
    {
        if(!collected)
        {
            audioSource.PlayOneShot(coinSound, 0.5f);
            collected = true;
            spi.color = new Color(1f,1f,1f,0f);
            player.GetComponent<Movement>().grounded = true;
            boxCollider.enabled = false;
            player.GetComponent<Movement>().jumpForce = 650f;
            GetComponent<ParticleSystem>().Play();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.transform.gameObject;
            Collect();
        }
    }
    void Respawn()
    {
        timeCount += Time.deltaTime;
        if(timeCount >= respawnTime - 1 && timeCount < respawnTime)
        {
            spi.color = new Color(1f,1f,1f,opacity);
            opacity += Time.deltaTime;
            if(opacity > 1)
            {
                opacity = 1f;
            }
        }
        else if(timeCount >= respawnTime)
        {
            boxCollider.enabled = true;
            spi.color = new Color(1f,1f,1f,1f);
            opacity = 0;
            timeCount = 0;
            collected = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(collected)
        {
            Respawn();
        }
    }
}
