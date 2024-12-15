using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionHover : Button
{
    [SerializeField] ButtonMatrix buttonMatrix;
    [SerializeField] ResolutionButton resolutionButton;
    [SerializeField] bool down = false;

    public override void Hover()
    {
        if(!down)
        {
            resolutionButton.index++;
            resolutionButton.index = Mathf.Clamp(resolutionButton.index, 0, resolutionButton.resolutions.Length - 1);
            buttonMatrix.Navigate('W');
        }
        else
        {
            resolutionButton.index--;
            resolutionButton.index = Mathf.Clamp(resolutionButton.index, 0, resolutionButton.resolutions.Length - 1);
            buttonMatrix.Navigate('E');
        }
    }
    public override void Unhover()
    {
        return;
    }
}
