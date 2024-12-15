using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryHover : Button 
{
    [SerializeField] SkinMatrix matrix;
    [SerializeField] AccessoryDisplay display;
    [SerializeField] bool left; //true - go left, decrease by 1, false - go right, increase by 1

    public override void Hover()
    {
        if(!left)
        {
            display.NextAcc();
            matrix.Navigate('W');
        }
        else
        {
            display.PrevAcc();
            matrix.Navigate('E');
        }
    }
    public override void Unhover()
    {
        return;
    }
}