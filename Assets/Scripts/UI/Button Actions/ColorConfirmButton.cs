using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorConfirmButton : Button
{
    [SerializeField] ColorMatrix colorMatrix;
    public override void Activate()
    {
        colorMatrix.Close();
    }
}
