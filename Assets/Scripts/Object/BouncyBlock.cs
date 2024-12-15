using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBlock : MonoBehaviour
{
    MultiplayerHandler multiplayer;
    GameObject player1;
    GameObject player2;
    Movement movement;
    GameObject player;
    Rigidbody2D playerRigidBody;
    BounceAnimation bounceAnimation;
    float playerVelocityY = 0;
    [SerializeField] float bounciness = .8f;
    [SerializeField] bool ignoreY;
    string direction = "north";
    [SerializeField] float stopLength = .25f;
    float timeCount = 0;
    //float ejectTimeCount = 0;
    bool ejecting = false;
    float ejectVelocity = 0;

    //Gets cardinal direction of player relative to each side of the bouncy block. The function does not work if the block is rotated so please don't do that thanks.
    string GetDirection()
    {
        if(ignoreY)
        {
            if(player.transform.position.x > transform.position.x)
            {
                return "east";
            }
            else
            {
                return "west";
            }
        }
        //Diagonal lines from the center of block. 
        float line1y = (transform.localScale.y/transform.localScale.x) * (player.transform.position.x - transform.position.x) + transform.position.y;
        float line2y = (transform.localScale.y/transform.localScale.x) * -1 * (player.transform.position.x - transform.position.x) + transform.position.y;
        if(player.transform.position.y >= line1y && player.transform.position.y >= line2y)
        {
            return "north"; //This is also the default if player and block share the same coordinates.
        }
        else if(player.transform.position.y > line1y && player.transform.position.y < line2y)
        {
            return "west";
        }
        else if(player.transform.position.y < line1y && player.transform.position.y > line2y)
        {
            return "east";
        }
        else if(player.transform.position.y <= line1y && player.transform.position.y <= line2y)
        {
            return "south";
        }
        else { return "north"; } //Reaching this return statement shouldn't be possible, but it's here just in case.
    }
    GameObject NearestPlayer()
    {
        if(multiplayer.dropIn)
        {
            player2 = GameObject.Find("Player 2");
            if(Vector2.Distance(transform.position, player1.transform.position) > Vector2.Distance(transform.position, player2.transform.position))
            {
                playerRigidBody = player2.GetComponent<Rigidbody2D>();
                return player2;
            }
            else
            {
                playerRigidBody = player1.GetComponent<Rigidbody2D>();
                return player1;
            }
        }
        else if(player == null)
        {
            return GameObject.Find("Player");
        }
        return player;
    }
    void ApplyForce(string direction)
    {
        if(direction == "north" || direction == "south")
        {
            playerRigidBody.velocity = new Vector3(0 , -1f * playerVelocityY * bounciness);
        }
        else
        {
            ejecting = true;
            if(direction == "east")
            {
                ejectVelocity = 20 * bounciness;
            }
            else if(direction == "west")
            {
                ejectVelocity = -20 * bounciness;
            }
        }
    }
    void OnDisable() //Shoddy solution to remedy old system developed for old movement system.
    {
        ejecting = false; 
    }
    //This method had to have been made because, while the y axis is handled by the physics system, the x axis is controlled directly by the keys/controller.
    //This is a nightmare and I hate it, but the pain is over.
    void Eject()
    {
        if(Mathf.Abs(ejectVelocity) < 2.5f && movement.grounded)
        {  
            ejecting = false;
            return;
        }
        if(direction == "west")
            if(ejectVelocity < 0)
            {
                if(!movement.leftColliderCheck.isTouching)
                {
                    player.transform.position = new Vector3(player.transform.position.x + ejectVelocity * Time.deltaTime, player.transform.position.y);
                    ejectVelocity += 20 * Time.deltaTime;
                }
                else
                {
                    ejectVelocity = 0;
                    ejecting = false;
                }
            }
            else
            {
                ejecting = false;
            }
        if(direction == "east")
        {
            if(ejectVelocity > 0)
            {
                if(!movement.rightColliderCheck.isTouching)
                {
                    player.transform.position = new Vector3(player.transform.position.x + ejectVelocity * Time.deltaTime, player.transform.position.y);
                    ejectVelocity -= 20 * Time.deltaTime;
                }
                else
                {
                    ejectVelocity = 0;
                    ejecting = false;
                }
            }
            else
            {
                ejecting = false;
            }
        }
    }
    //Bounces the player up when they hit the block.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            movement = collision.gameObject.GetComponent<Movement>();
            direction = GetDirection();
            ApplyForce(direction);
            if(bounceAnimation != null) bounceAnimation.Play(direction);
            
        }
    }
    //Stops animation if player stands on block for more than stopLength.
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            timeCount += Time.deltaTime;
            if(bounceAnimation != null && timeCount >= stopLength)
            {
                bounceAnimation.Stop();
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            timeCount = 0f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player1 = player;
        multiplayer = GameObject.Find("EventSystem").GetComponent<MultiplayerHandler>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        bounceAnimation = GetComponent<BounceAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        player = NearestPlayer(); //Assigns closest player to player variable if multiplayer.
        //playerVelocity is calculated here to get it the frame before collision instead of after -- where it would be effectively 0.
        playerVelocityY = playerRigidBody.velocity.y;
        if(ejecting)
        {
            Eject();
        }
    }
}
