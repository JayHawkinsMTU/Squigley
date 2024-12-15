using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorEvent : MessageEvent
{
    public LockedDoor door;
    public override void Event(GameObject p)
    {
        door.Unlock();
    }
}
