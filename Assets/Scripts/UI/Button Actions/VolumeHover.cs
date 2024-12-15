using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHover : Button
{
    [SerializeField] ButtonMatrix buttonMatrix;
    [SerializeField] VolumeHandler volumeHandler;
    [SerializeField] bool down = false;

    public override void Hover()
    {
        if(!down)
        {
            buttonMatrix.Navigate('W');
            volumeHandler.UpdateVolume(.1f);
        }
        else
        {
            buttonMatrix.Navigate('E');
            volumeHandler.UpdateVolume(-.1f);
        }
    }
    public override void Unhover()
    {
        return;
    }
}
