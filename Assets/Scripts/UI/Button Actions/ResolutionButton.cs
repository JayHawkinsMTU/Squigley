using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionButton : Button
{
    public TMP_Text rDisplay;
    [System.NonSerialized] public Resolution[] resolutions;
    public int index;
    [System.NonSerialized] public Resolution currentRes;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        currentRes = Screen.currentResolution;
        index = resolutions.Length - 1;
        for(int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == currentRes.width && resolutions[i].height == currentRes.height && resolutions[i].refreshRateRatio.value == currentRes.refreshRateRatio.value)
            {
                index = i;
            }
        }
        rDisplay.text = currentRes.width + "x" + currentRes.height + " " + currentRes.refreshRateRatio + "hz";
    }

    public override void Hover()
    {
        currentRes = resolutions[index];
        if(GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().color = Color.yellow;
        }
        rDisplay.text = currentRes.width + "x" + currentRes.height + " " + currentRes.refreshRateRatio + "hz";
        Screen.SetResolution(currentRes.width, currentRes.height, Screen.fullScreen);
    }

    public override void Activate()
    {
        return;
    }
}
