using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHover : Button
{
    [SerializeField] ColorMatrix colorMatrix;
    [SerializeField] char comp;
    [SerializeField] bool down;

    public override void Hover()
    {
        if(!down)
        {
            colorMatrix.Navigate('S');
            colorMatrix.BumpComponent(comp, .1f);
        }
        else
        {
            colorMatrix.Navigate('N');
            colorMatrix.BumpComponent(comp, -.1f);
        }
    }
    public override void Unhover()
    {
        return;
    }
}
