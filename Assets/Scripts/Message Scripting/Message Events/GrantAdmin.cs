using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantAdmin : MessageEvent
{
    public override void Event(GameObject p)
    {
        UICoinHandler.admin = true;
        UtilityText.primaryInstance.DisplayMsg("ADMIN ACCESS GRANTED FOR: SQUIGLEY", Color.green);
    }
}
