using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerHandler : MonoBehaviour
{
    public bool dropIn = false;
    public bool multiplayer = false;
    [SerializeField] GameObject player1Grapple;
    LineRenderer grappleRender;
    followDaGuy player1Camera;
    [SerializeField] Movement player1, player2;
    public bool[] req = new bool[2]; //An array of two booleans representing which players are requesting to grapple.
    [SerializeField] string [] tutorialmsg;
    bool msgDisplayed = false;
    
    //Grapple
    /*public GameObject groundedPlayer;
    public GameObject airbournePlayer;*/
    public bool grappling = false;
    [SerializeField] float grappleSpeed = 3;

    //Audio
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2; //For grapple audio
    [SerializeField] AudioClip dropInClip;
    [SerializeField] AudioClip dropOutClip;

    //Text
    [SerializeField] UtilityText text;

    void DropIn()
    {
        dropIn = true;
        player1Grapple.SetActive(true);
        grappleRender = player1Grapple.GetComponent<LineRenderer>();
        player2.gameObject.SetActive(true);
        player2.transform.position = new Vector3(player1.transform.position.x + 3f, player1.transform.position.y + 1f, 0f);
        Message.globalMessage = tutorialmsg;
        Message.newMessage = true;
        audioSource.PlayOneShot(dropInClip, 0.5f);
    }
    void DropOut()
    {
        dropIn = false;
        player1Grapple.SetActive(false);
        player2.gameObject.SetActive(false);
        audioSource.PlayOneShot(dropOutClip, 0.5f);
    }
    void ActivateGrapple()
    {
        if(!grappling) audioSource2.Play();
        grappling = true;
        /*airbournePlayer.GetComponent<Movement>().enabled = false;
        airbournePlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
        airbournePlayer.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);*/
        grappleRender.enabled = true;
        grappleRender.startColor = player1.GetComponent<SpriteRenderer>().color;
        grappleRender.startColor = new Color(grappleRender.startColor.r, grappleRender.startColor.g, grappleRender.startColor.b, 0.4f);
        grappleRender.endColor = player2.GetComponent<SpriteRenderer>().color;
        grappleRender.endColor = new Color(grappleRender.endColor.r, grappleRender.endColor.g, grappleRender.endColor.b, 0.4f);


    }
    void DeactivateGrapple()
    {
        grappling = false;
        grappleRender.enabled = false;
        EnableProperties(player1);
        EnableProperties(player2);
        audioSource2.Stop();

        
    }
    void GrappleTo(Movement from, Movement to, float distance)
    {
        DisableProperties(from);
        from.transform.position = Vector2.MoveTowards(from.transform.position, to.transform.position, Time.deltaTime * grappleSpeed * (distance / 3));
    }
    void EnableProperties(Movement p)
    {
        if(!p.enabled) return; //Do nothing if already enabled
        p.enabled = true;
        Rigidbody2D prb = p.GetComponent<Rigidbody2D>();
        prb.gravityScale = 2;
    }
    void DisableProperties(Movement p)
    {
        if(!p.enabled) return; //Do nothing if already disabled
        p.enabled = false;
        Rigidbody2D prb = p.GetComponent<Rigidbody2D>();
        prb.gravityScale = 0;
        prb.velocity = new Vector3(0f, 0f, 0f);
    }
    void Grapple()
    {
        //Debug.DrawRay(groundedPlayer.transform.position, airbournePlayer.transform.position, Color.white);
        Vector3[] positions = {new Vector3(player1.transform.position.x, player1.transform.position.y, 1), new Vector3(player2.transform.position.x, player2.transform.position.y, 1)};
        grappleRender.SetPositions(positions);
        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        //airbournePlayer.transform.position = Vector2.MoveTowards(airbournePlayer.transform.position, groundedPlayer.transform.position, Time.deltaTime * grappleSpeed * (distance / 3));
        if(!player1.grounded) GrappleTo(player1, player2, distance);
        if(!player2.grounded) GrappleTo(player2, player1, distance);
        player1Camera.UpdateCamera();
    }
    void CheckMP()
    {
        var controllers = Input.GetJoystickNames();
        if(controllers.Length > 1) multiplayer = true;
        else multiplayer = false;
    }
    void Start()
    {
        if(player1 != null && player1.pCamera != null) player1Camera = player1.pCamera;
    }
    // Update is called once per frame
    void Update()
    {
        CheckMP();
        if(multiplayer)
        {
            if(!dropIn && !msgDisplayed)
            {
                text.DisplayMsg("P2 Start!", Color.grey);
                msgDisplayed = true;
            }
            if(dropIn && msgDisplayed)
            {
                text.CloseMsg();
                msgDisplayed = false;
            }
            if(GameInput.DropIn())
            {
                if(dropIn)
                {
                    DropOut();
                }
                else
                {
                    DropIn();
                }
            }
            if(req[0] && req[1]) ActivateGrapple();
            else if(grappling) DeactivateGrapple();
            if(grappling) 
            {
                Grapple();
            }
        }
    }
}
