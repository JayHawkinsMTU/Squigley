using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoclipMovement : MonoBehaviour
{
    [SerializeField] GameObject areaLoader;
    [SerializeField] bool noClipOnStart = false;
    //Modes
    private const int GLIDE = 0; //Smoothly glide and slowly decelerate.
    private const int FLAT = 1; //Rigidly
    private const int LOCK = 2;
    private int mode = GLIDE;
    private float glideForce = 1000f;
    private float glideDecay = 0.5f;
    private float flatVelocity = 5f;
    private int xDir = 0;
    private int yDir = 0;
    BoxCollider2D playerCollider;
    Movement movement;
    Rigidbody2D playerRigidbody;
    followDaGuy playerCamera;
    [SerializeField] GameObject triggers;
    public void ToggleNoclip()
    {
        enabled = !enabled;
        playerCollider.enabled = !playerCollider.enabled;
        movement.enabled = !movement.enabled;
        areaLoader.SetActive(enabled);
        if(!enabled)
        {
            playerRigidbody.gravityScale = 2;
            triggers.SetActive(true);
        } 
        else
        {
            playerRigidbody.gravityScale = 0;
            triggers.SetActive(false);
        }
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = 0f;
        transform.eulerAngles = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        movement = GetComponent<Movement>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCamera = movement.pCamera;
        enabled = false;
        areaLoader.SetActive(false);
        if(noClipOnStart)
        {
            ToggleNoclip();
        }
    }
    void LockToMove()
    {
        //Locks to nearest integer
        transform.position = new Vector2((int) transform.position.x, (int) transform.position.y);
        if(GameInput.UILeft())
        {
            transform.Translate(-1, 0, 0);
        }
        else if(GameInput.UIRight())
        {
            transform.Translate(1, 0, 0);
        }
        if(GameInput.UIUp())
        {
            transform.Translate(0, 1, 0);
        }
        else if(GameInput.UIDown())
        {
            transform.Translate(0, -1, 0);
        }
    }
    void Move(Vector2 direction)
    {
        switch(mode)
        {
            case GLIDE:
                playerRigidbody.AddForce(direction * glideForce * Time.deltaTime);
                break;
            case FLAT:
                transform.Translate(direction.x * flatVelocity * Time.deltaTime, direction.y * flatVelocity * Time.deltaTime, 0);
                break;
            case LOCK:
                LockToMove();
                break;
            default:
                Debug.LogError("INVALID NOCLIP MODE");
                break;
        }
        playerRigidbody.velocity = playerRigidbody.velocity - playerRigidbody.velocity * (Time.deltaTime / glideDecay);
    }
    // Update is called once per frame
    void Update()
    {
        if(GameInput.NoclipFlat())
        {
            playerRigidbody.velocity = Vector2.zero;
            mode = FLAT;
        }
        else if(GameInput.NoclipLock())
        {
            playerRigidbody.velocity = Vector2.zero;
            mode = LOCK;
        }
        else
        {
            mode = GLIDE;
        }
        if(Time.timeScale == 0) return;
        if(GameInput.MoveLeft())
        {
            xDir = -1;
        }
        else if(GameInput.MoveRight())
        {
            xDir = 1;
        }
        else
        {
            xDir = 0;
        }
        if(GameInput.MoveUp())
        {
            yDir = 1;
        }
        else if(GameInput.MoveDown())
        {
            yDir = -1;
        }
        else
        {
            yDir = 0;
        }
        Move(new Vector2(xDir, yDir));

        if(playerCamera != null) //Camera should only be null if it's player 2.
            playerCamera.UpdateCamera();
    }
}
