using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartenderLogic : MessageEvent, IDataPersistence
{
    public bool hasId = false;
    [SerializeField] NewMessageHandler msg;
    [SerializeField] string hasIdMsg = "How about a drink?";
    // Start is called before the first frame update
    public override void Event(GameObject p)
    {
        if(hasId)
        {
            msg.message[^1] = hasIdMsg;
        }
    }

    public void LoadData(SaveData data)
    {
        this.hasId = data.hasId;
    }

    public void SaveData(ref SaveData data)
    {
        data.hasId = this.hasId;
    }
}
