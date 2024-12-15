using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeHandler : Button
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] RectTransform bar;
    private float volume;

    public void UpdateVolume(float difference)
    {
        volume += difference;
        volume = Mathf.Clamp(volume, 0, 1);
        audioMixer.SetFloat("volume", (volume * 80) - 80);
        if(bar != null)
            bar.localScale = new Vector3(volume, 1, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.GetFloat("volume", out volume);
        volume = (volume + 80) / 80;
        UpdateVolume(0);
    }
    public override void Activate()
    {
        return;
    }
}
