using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    private const float THROW_VELOCITY = 8.25f;
    private const float SAFETY_DIST = 1.75f; //Should at least be out of parry distance
    private AudioSource audioSource;
    [SerializeField] AudioClip throwSound;
    public GameObject player;
    int playerid;
    SpriteRenderer playerSpi; //Used in order to get orientation.
    [SerializeField] GameObject projectile;
    SpriteRenderer spi;
    Sprite sprite;
    private int orientation;
    
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        sprite = projectile.GetComponent<SpriteRenderer>().sprite;
        spi.sprite = this.sprite;
        playerid = player.GetComponent<Movement>().playerID;
        audioSource = GetComponent<AudioSource>();
        if(player != null) Enable(player);
    }

    public void Enable(GameObject p)
    {
        player = p;
        playerSpi = p.GetComponent<SpriteRenderer>();
    }
    
    private void GetOrientation()
    {
        if(playerSpi.flipX)
        {
            orientation = 1;
        }
        else
        {
            orientation = -1;
        }
        transform.localPosition = new Vector3(orientation * 0.65f, transform.localPosition.y, transform.localPosition.z);
    }
    private void Fire(int x, int y)
    {
        audioSource.PlayOneShot(throwSound, 0.65f);
        float height = transform.position.y + 0.25f;
        if(y > 0) height = transform.position.y + SAFETY_DIST;
        else if(y < 0) height = transform.position.y - SAFETY_DIST;
        Instantiate(projectile, new Vector3(transform.position.x + x * SAFETY_DIST, height, transform.position.z), Quaternion.identity).GetComponent<Projectile>().Fire(x * THROW_VELOCITY, y * THROW_VELOCITY);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        GetOrientation();
        if(GameInput.Interact(playerid))
        {
            if(GameInput.MoveUp(playerid))
            {
                Fire(0, 1);
            }
            else if(GameInput.MoveDown(playerid))
            {
                Fire(0, -1);
            }
            else
            {
                Fire(orientation, 0);
            }
        }
    }
}
