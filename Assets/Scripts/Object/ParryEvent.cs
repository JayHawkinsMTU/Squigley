using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryEvent : Parryable
{
    public MessageEvent msgEvent;
    public override void Parry()
    {
        msgEvent.Event(null);
    }
}
