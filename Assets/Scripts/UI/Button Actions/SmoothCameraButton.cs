using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraButton : Button
{
    [SerializeField] GameObject tickMark;
    public bool smoothCamera = false;
    // Start is called before the first frame update
    void Start()
    {
        tickMark.SetActive(smoothCamera);
    }

    public override void Activate()
    {
        smoothCamera = !smoothCamera;
        tickMark.SetActive(smoothCamera);
    }
}
