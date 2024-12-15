using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinSound;
    public SpriteRenderer spi;
    public GameObject eventSystem;
    public GameObject player;
    bool collected = false;
    public int value = 1;
    [SerializeField] bool oblivion = false;

    //Communicates with Event System every time a coin is collected.
    void Collect()
    {
        if(!collected)
        {
            collected = true;
            spi.enabled = false;
            audioSource.PlayOneShot(coinSound, 0.5f);
            if(!oblivion)
            {
                UICoinHandler ui = eventSystem.GetComponent<UICoinHandler>();
                ui.coinCount += value;
                if(ui.coinCount > 999)
                {
                    AchievementManager.GetAchievement("MAX_COINS");
                }
            }
            else
            {
                eventSystem.GetComponent<ScoreHandler>().CollectCoin(value);
            }
            eventSystem.GetComponent<UICoinHandler>().totalCoinsCollected += value;
        }
    }
    public virtual void Respawn()
    {
        collected = false;
        spi.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Collect();
        }
    }
    void Start()
    {
        //Grabs necessary components.
        spi=GetComponent<SpriteRenderer>();
        eventSystem = GameObject.Find("EventSystem");
        player = GameObject.Find("Player");
    }
    /*void Update()
    {
        //Checks every frame to see if the player died. This is probably an extreme waste of resources.
        if(player.GetComponent<Movement>().dead)
        {
            Respawn();
        }
    }*/
}
