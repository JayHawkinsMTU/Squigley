using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraUpgrades : MonoBehaviour, IDataPersistence
{
    [SerializeField] Camera playerCamera;
    [SerializeField] MultiplayerHandler multiplayer;
    [SerializeField] TMP_Text sizeDisplay;
    AudioSource audioSource;
    [SerializeField] AudioClip zoomOut;
    [SerializeField] AudioClip zoomIn;
    
    public int defaultSize = 8;
    public int minSize = 7;
    public int maxSize = 9;
    public static int currentSize = 8;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera.orthographicSize = currentSize;
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        sizeDisplay.text = (currentSize - 7).ToString();
    }
    public void ZoomOut() 
    {
        int initSize = currentSize;
        if(!multiplayer.dropIn)
        {
            if(currentSize < maxSize) currentSize++;
        }
        else
        {
            if(currentSize < 25) currentSize++;
        }
        playerCamera.orthographicSize = currentSize;
        UpdateDisplay();
        if(currentSize != initSize) 
        {
            audioSource.PlayOneShot(zoomOut, 0.5f);
        }
    }
    public void ZoomIn()
    {
        int initSize = currentSize;
        if(currentSize > minSize) currentSize--;
        playerCamera.orthographicSize = currentSize;
        UpdateDisplay();
        if(currentSize != initSize) 
        {
            audioSource.PlayOneShot(zoomOut, 0.5f);
        }
    }
    public void LoadData(SaveData data)
    {
        this.maxSize = data.maxSize;
        if(data.currentSize <= maxSize)
        {
            currentSize = data.currentSize;
            playerCamera.orthographicSize = data.currentSize;
        }
        followDaGuy cam = playerCamera.GetComponent<followDaGuy>();
        cam.smoothCamera = data.smoothCamera;
        cam.InstantToPlayer();
    }
    public void SaveData(ref SaveData data)
    {
        data.maxSize = this.maxSize;
        if(!multiplayer.dropIn) data.currentSize = currentSize; //Current size will not save when multiplayer to prevent exploits.
    }
}
