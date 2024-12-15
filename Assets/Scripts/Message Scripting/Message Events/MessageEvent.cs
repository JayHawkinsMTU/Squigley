using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageEvent : MonoBehaviour
{
    public int index;
    public virtual void Event(GameObject p)
    {
        Debug.Log("Message Event Activated at message index: " + index);
    }
}
