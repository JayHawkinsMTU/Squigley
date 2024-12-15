using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakVase : MessageEvent
{
    [SerializeField] Vase vase;
    public override void Event(GameObject p)
    {
        vase.Interact(p);
    }
}
