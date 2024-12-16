using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] tracks;
    public string[] trackNames;
    private static int trackId = 0;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayTrack();
    }

    private void PlayTrack()
    {
        audioSource.clip = tracks[trackId];
        audioSource.Play();
        if(UtilityText.primaryInstance != null) UtilityText.primaryInstance.DisplayMsg(trackNames[trackId], Color.green);
    }

    private void NextTrack()
    {
        trackId = (trackId + 1) % tracks.Length; 
        PlayTrack();
    }

    private void Volume(float delta)
    {
        audioSource.volume += delta;
    }

    void Update()
    {
        if(GameInput.Pause())
        {
            NextTrack();
        }
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            //Volume up
            Volume(.1f);
        }
        if(Input.GetKeyDown(KeyCode.Minus))
        {
            //Volume down
            Volume(-.1f);
        }
    }
}
