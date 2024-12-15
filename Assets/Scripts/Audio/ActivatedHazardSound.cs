using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedHazardSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip click;
    public ActivatedMovingBridge amb;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(amb.active)
        {
            audioSource.enabled = true;
        }
        else if(audioSource.enabled)
        {
            audioSource.enabled = false;
        }
    }
}
