using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : Button
{
    [SerializeField] string logMessage = "ButtonAction";
    public override void Activate()
    {
        Debug.Log(logMessage);
    }
}
