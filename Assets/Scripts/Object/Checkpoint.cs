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
        //UtilityText.primaryInstance.DisplayMsg("Checkpoint " + id + " Activated", Color.white);
        DeactivateAll(); //Deactivates all other checkpoints to prevent confusion.
        //UtilityText.primaryInstance.DisplayMsg("1", Color.white);
        if(lives == 0)
        {
            lives = this.lives;
        }
        //UtilityText.primaryInstance.DisplayMsg("2", Color.white);
        player = GameObject.Find("Player");
        //UtilityText.primaryInstance.DisplayMsg("3", Color.white);
        Movement m = player.GetComponent<Movement>();
        //UtilityText.primaryInstance.DisplayMsg("4", Color.white);
        m.checkPointLives = lives;
        //UtilityText.primaryInstance.DisplayMsg("5", Color.white);
        active = true;
        //UtilityText.primaryInstance.DisplayMsg("6", Color.white);
        spi.enabled = true;
        //UtilityText.primaryInstance.DisplayMsg("7", Color.white);
        m.checkPointCoords = transform.position;
        //UtilityText.primaryInstance.DisplayMsg("8", Color.white);
        audioSource.PlayOneShot(activationSound, 0.5f);
        //UtilityText.primaryInstance.DisplayMsg("9", Color.white);
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
