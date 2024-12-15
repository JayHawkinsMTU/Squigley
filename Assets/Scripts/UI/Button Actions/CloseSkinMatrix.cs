using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSkinMatrix : Button
{
    [SerializeField] SkinMatrix skinMatrix;
    public override void Activate()
    {
        skinMatrix.Close();
    }
}
