using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBox : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip woahSound;

    /*[SerializeField]*/ float rotationDegrees;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.transform.eulerAngles = new Vector3(0, 0, rotationDegrees);
            player.GetComponent<Movement>().fastfallCharge = true;
            audioSource.PlayOneShot(woahSound, .5f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.localRotation.eulerAngles.z);
        rotationDegrees = transform.localRotation.eulerAngles.z;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        rotationDegrees = transform.localRotation.eulerAngles.z;
    }
}
