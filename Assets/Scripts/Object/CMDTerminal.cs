using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDTerminal : Box
{
    public DebugTerminal console;
    public string command;

    public override void Interact(GameObject p)
    {
        console.EnterCmd(command);
    }
}
