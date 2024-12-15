using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSettings : MonoBehaviour, IDataPersistence
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] ResolutionButton resolutionButton;
    [SerializeField] SpeedrunTimerButton speedrunTimerButton;
    [SerializeField] SmoothCameraButton smoothCameraButton;
    private static bool loaded = false;
    public void SaveData(ref SaveData data)
    {
        audioMixer.GetFloat("volume", out data.volume);
        data.volume = (data.volume + 80) / 80;
        data.fullScreen = Screen.fullScreen;
        if(resolutionButton != null)
            data.resolution = resolutionButton.currentRes;
        if(speedrunTimerButton != null)
            data.speedrunTimer = speedrunTimerButton.visible;
        if(smoothCameraButton != null)
            data.smoothCamera = smoothCameraButton.smoothCamera;
    }
    public void LoadData(SaveData data)
    {
        // Prevents loading old data twice
        if(loaded) return;
        loaded = true;
        audioMixer.SetFloat("volume", (data.volume * 80) - 80);
        Screen.fullScreen = data.fullScreen;
        //The line below causes a 0x0 resolution. BAD
        //It appears that unity remembers the last resolution set on its own. Very nice!
        //Screen.SetResolution(data.resolution.width, data.resolution.height, data.fullScreen);
        if(speedrunTimerButton != null) {
            speedrunTimerButton.visible = data.speedrunTimer;
            speedrunTimerButton.UpdateTickmark();
        }
        if(smoothCameraButton != null)
            smoothCameraButton.smoothCamera = data.smoothCamera;
        if(resolutionButton != null)
            resolutionButton.rDisplay.text = data.resolution.width + "x" + data.resolution.height + " " + data.resolution.refreshRateRatio + "hz";
    }
}
