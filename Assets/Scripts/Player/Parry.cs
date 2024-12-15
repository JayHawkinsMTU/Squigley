using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour, IDataPersistence
{
    public static bool unlocked = false;
    private const float JUMP_VEL = 11f; //Velocity to be added to rigidbody when parry is successful.
    private const float DURATION = 0.075f; //Maximum length of parry.
    private const float COOLDOWN = .5f;
    private Wait durWait = new Wait(DURATION);
    private Wait coolWait = new(COOLDOWN);
    [SerializeField] Rigidbody2D playerRigidBody;
    [SerializeField] CircleCollider2D parryZone;
    SpriteRenderer spi;
    [SerializeField] Movement movement;
    AudioSource audioSource;
    [SerializeField] AudioClip woosh; //Sound that plays when activating parry.
    [SerializeField] AudioClip clang; //Sound that plays on succesful parry.
    private int playerID = 1;
    private bool active = false;
    private bool charge = true; //Resets when grounded.
    private bool cooled = true;
    private void Activate()
    {
        spi.enabled = true;
        active = true;
        charge = false;
        cooled = false;
        parryZone.enabled = true;
        audioSource.PlayOneShot(woosh, 0.5f);
    }
    private void Deactivate()
    {
        active = false;
        spi.enabled = false;
        parryZone.enabled = false;
    }
    private void ParryObject(GameObject g)
    {
        Parryable p = g.GetComponent<Parryable>();
        if(p != null)
        {
            p.Parry();
        }
        if(playerRigidBody.velocity.y < 0) playerRigidBody.velocity = Vector2.zero;
        if(g.transform.position.y < transform.position.y)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.y + JUMP_VEL);
        }
        audioSource.PlayOneShot(clang, 0.5f);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Hazards" || collider.GetComponent<Parryable>() != null)
        {
            ParryObject(collider.gameObject);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        playerID = movement.playerID;
        audioSource = GetComponent<AudioSource>();
        spi = GetComponent<SpriteRenderer>();
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if(active) //Wait for parry to deactivate
        {
            durWait.Iterate();
            if(durWait.Complete())
            {
                Deactivate();
            }
        }
        if(!cooled) //Wait for cooldown. Starts upon activation
        {
            coolWait.Iterate();
            if(coolWait.Complete())
            {
                cooled = true;
            }
        }
        else if(!charge) //Recharge
        {
            charge = movement.grounded;
        }
        if(charge && !active) //Allow for user to parry
        {
            // Shouldn't parry if player is holding a projectile.
            if(GameInput.Interact(playerID) && transform.parent.GetComponentInChildren<ProjectileThrow>() == null)
            {
                Activate();
            }
        }
    }

    public void LoadData(SaveData data)
    {
        gameObject.SetActive(data.parryUnlocked);
        Parry.unlocked = data.parryUnlocked;
    }

    public void SaveData(ref SaveData data)
    {
        data.parryUnlocked = Parry.unlocked;
    }
}
