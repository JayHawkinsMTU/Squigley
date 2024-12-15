using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour, Interactable
{
    public GameObject player;
    public GameObject eventSystem;
    bool available;
    bool broken;

    public SpriteRenderer spi;
    public Sprite intact;
    public Sprite broke;
    public AudioSource audioSource;
    public AudioClip breakSound;
    public AudioClip coinSound;

    [SerializeField] int chance = 3; //1/3 chance
    [SerializeField] int minValue = 1; //Inclusive
    [SerializeField] int maxValue = 5; //Exclusive
    [SerializeField] bool oblivion;
    public void Respawn()
    {
        broken = false;
        spi.sprite = intact;
    }
    public void Interact(GameObject p)
    {
        Break();
    }
    void Break()
    {
        if(!broken)
        {
            eventSystem.GetComponent<UICoinHandler>().vasesBroken++;
            audioSource.PlayOneShot(breakSound, 0.75f);
            broken = true;
            spi.sprite = broke;
            if(Random.Range(0, chance) == 1)
            {
                Collect(Random.Range(minValue, maxValue));
            }
        }
    }
    void Collect(int value)
    {
        audioSource.PlayOneShot(coinSound, 0.5f);
        if(!oblivion)
        {
            eventSystem.GetComponent<UICoinHandler>().coinCount += value;
        }
        else
        {
            eventSystem.GetComponent<ScoreHandler>().CollectCoin(value);
        }
    }
    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log(available);
            available = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            available = false;
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    /*void Update()
    {
        if(player.GetComponent<Movement>().dead)
        {
            Respawn();
        }
    }*/
}
