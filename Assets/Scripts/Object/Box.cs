using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, Interactable
{
    public AudioSource audioSource;
    [SerializeField] AudioClip bind;
    private bool bound = false;
    public GameObject player;
    private Rigidbody2D playerRigidbody;
    public int playerID = 1;

    public virtual void Interact(GameObject p)
    {
        player = p;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        p.GetComponent<Movement>().boundTo = this;
        
        audioSource.PlayOneShot(bind, 0.5f);
        player.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        player.transform.localScale = new Vector3(1,1,1);
        player.transform.eulerAngles = new Vector3(0, 0, 0);
        if(!bound)
        {
            bound = true;
            player.GetComponent<Movement>().jumpForce = 500f;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = 0;
            playerRigidbody.gravityScale = 0;
        }
        else
        {
            Unbind();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Unbind()
    {
        bound = false;
        player.GetComponent<Movement>().boundTo = null;
        playerRigidbody.gravityScale = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(bound)
        {
            player.transform.position = transform.position;
            
            //Releases player on jump or fastfall.
            if(GameInput.Jump(playerID))
            {
                player.GetComponent<Movement>().grounded = true;
                audioSource.PlayOneShot(bind, 0.5f);
                Unbind();
            }
            else if(GameInput.FastFall(playerID))
            {
                Movement m = player.GetComponent<Movement>();
                m.fastfallCharge = true;
                Unbind();
                m.FastFall();
                audioSource.PlayOneShot(bind, 0.5f);
            }
        }   
    }
}
