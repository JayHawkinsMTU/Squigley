using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IDataPersistence
{
    public SpriteRenderer spi;
    public GameObject player;
    AudioSource audioSource;
    [SerializeField] AudioClip activationSound;
    [SerializeField] bool active = false;
    [SerializeField] int lives = 1;
    [SerializeField] int id = 0; //0 Should correspond to debug checkpoints or no checkpoint.
    [SerializeField] bool load = true;
    public void Activate(int lives = 0)
    {
        DeactivateAll(); //Deactivates all other checkpoints to prevent confusion.
        if(lives == 0)
        {
            lives = this.lives;
        }
        player = GameObject.Find("Player");
        Movement m = player.GetComponent<Movement>();
        m.checkPointLives = lives;
        active = true;
        spi.enabled = true;
        m.checkPointCoords = transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(activationSound, 0.5f);
        gameObject.SetActive(true);
    }
    public static void DeactivateAll()
    {
        List<Checkpoint> allChecks = new List<Checkpoint>();
        allChecks.AddRange(FindObjectsOfType<Checkpoint>(true));
        foreach(Checkpoint c in allChecks)
        {
            c.Deactivate();
        }
    }
    void Deactivate()
    {
        active = false;
        spi.enabled = false;
        if(player != null) player.GetComponent<Movement>().checkPointCoords = new Vector3(0, 0, 0);
    }
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spi = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    /*void Update()
    {
        if(active)
        {
            if(player.GetComponent<Movement>().checkPointLives <= 0)
            {
                Deactivate();
            }
        }
    }*/

    public void LoadData(SaveData data)
    {
        //UtilityText.primaryInstance.DisplayMsg("Data loaded: " + id + ", " + data.currentCheckpointID, Color.white);
        if(load && data.currentCheckpointID == id)
        {
            Activate(data.checkPointLives);
            player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    public void SaveData(ref SaveData data)
    {
        if(active)
        {
            data.currentCheckpointID = id;
        }
        else if(data.currentCheckpointID == id)
        {
            data.currentCheckpointID = 0;
        }
    }
}
