using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpaceCorrection : Button
{
    public ButtonMatrix matrix;
    [SerializeField] bool left; //true - go left, decrease by 1, false - go right, increase by 1

    public override void Hover()
    {
        if(!left)
        {
            matrix.Navigate('W');
        }
        else
        {
            matrix.Navigate('E');
        }
    }
}
