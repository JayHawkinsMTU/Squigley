using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFader : MonoBehaviour
{
    public AudioSource[] tracks;
    public float fadeTime = 1;
    public int currentTrack = 0;

    [Range(0.0f, 1.0f)] public float volume;
    public void NextTrack()
    {
        if(currentTrack >= tracks.Length - 1) return;
        tracks[currentTrack].volume = volume;
        tracks[++currentTrack].volume = 0;
        StopAllCoroutines();
        StartCoroutine(Fade(currentTrack - 1, currentTrack));        
    }

    public void PrevTrack()
    {
        if(currentTrack < 1) return;
        tracks[currentTrack].volume = volume;
        tracks[--currentTrack].volume = 0;
        StopAllCoroutines();
        StartCoroutine(Fade(currentTrack + 1, currentTrack));    
    }

    IEnumerator Fade(int track1, int track2)
        {
            while(tracks[track1].volume > 0 || tracks[track2].volume < volume)
            {
                tracks[track1].volume = Mathf.Clamp(tracks[track1].volume - Time.deltaTime / (fadeTime * volume), 0, 1);
                tracks[track2].volume = Mathf.Clamp(tracks[track2].volume + Time.deltaTime / (fadeTime * volume), 0, 1);

                yield return new WaitForEndOfFrame();
            }
            tracks[track1].volume = 0;
            tracks[track2].volume = volume;
        }
    void Awake()
    {
        for(int i = 0; i < tracks.Length; i++)
        {
            if(i == currentTrack)
            {
                tracks[i].volume = volume;
            }
            else
            {
                tracks[i].volume = 0;
            }
        }
    }
}
