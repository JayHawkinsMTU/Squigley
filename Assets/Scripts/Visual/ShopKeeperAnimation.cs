using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperAnimation : MonoBehaviour
{
    public string currentAnimation = "Wave";
    public GameObject shop;
    SpriteRenderer spi;
    public Sprite neutral;
    public Sprite arms;
    public Sprite jump;
    public Sprite wave;
    float timeCount = 0;
    bool falling = false;
    bool inStand = false;

    public AudioSource audioSource;
    public AudioClip step;
    public AudioClip jumpSound;
    public AudioClip sit;
    // Start is called before the first frame update
    void Wave()
    {
        if(timeCount < 1)
        {
            spi.sprite = arms;
        }
        else if(timeCount < 2)
        {
            spi.sprite = wave;
        }
        else
        {
            timeCount = 0;
        }

        if(transform.localPosition.y > -.5f && !inStand)
        {
            spi.sprite = arms;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 4 * Time.deltaTime, 2);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -.5f, 2);
            falling = false;
        }
        if(inStand)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -.25f, 2);
        }
    }
    void Jump(float jumpHeight)
    {
        if(!inStand)
        {
            if(transform.localPosition.y < jumpHeight && !falling)
            {
                spi.sprite = jump;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 3 * Time.deltaTime);
            }
            else
            {
                falling = true;
            }
            if(falling)
            {
                if(transform.localPosition.y >= -.5f)
                {
                    spi.sprite = arms;
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 4 * Time.deltaTime);
                }
                else
                {
                    if(!inStand) audioSource.PlayOneShot(jumpSound, 0.25f);
                    transform.localPosition = new Vector3(transform.localPosition.x, -.5f);
                    falling = false;
                }
            }
        }
    }
    void ToStand(int speed)
    {
        spi.sprite = neutral;
        
        float distance = transform.position.x - shop.transform.position.x;
        if(distance > 0.1f)
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, 2);
            Jump(-.25f);
            if(timeCount > .5f)
            {
                if(!inStand) audioSource.PlayOneShot(step, 0.5f);
                timeCount = 0;
            }
        }
        else if(distance < -0.1f)
        {
            Jump(-.25f);
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 2);
            if(timeCount > .25f)
            {
                if(!inStand) audioSource.PlayOneShot(step, 0.5f);
                timeCount = 0;
            }
        }
        else
        {
            inStand = true;
            if(!inStand) audioSource.PlayOneShot(sit, 0.5f);
            transform.localPosition = new Vector3(transform.localPosition.x, -.25f, 2);
            currentAnimation = "Idle";
        }
        
    }
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        spi.sprite = neutral;
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        switch(currentAnimation)
        {
            case "Wave":
                Wave();
                if(falling)
                {
                    Jump(.5f);
                }
                break;
            case "Jump":
                Jump(.5f);
                break;
            case "ToStand":
                ToStand(4);
                break;
            default:
                break;
        }
        
    }
}
