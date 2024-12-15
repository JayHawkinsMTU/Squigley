using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenButton : Button
{
    [SerializeField] GameObject tickMark;
    // Start is called before the first frame update
    void Start()
    {
        tickMark.SetActive(Screen.fullScreen);
    }

    public override void Activate()
    {
        Screen.fullScreen = !Screen.fullScreen;
        tickMark.SetActive(!Screen.fullScreen);
    }
}
