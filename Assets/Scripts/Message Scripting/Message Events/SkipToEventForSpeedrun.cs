using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToEventForSpeedrun : MonoBehaviour, IDataPersistence
{
    bool speedrunTimer;
    NewMessageHandler newMessageHandler;
    [SerializeField] MessageEvent messageEvent;
    void Update()
    {
        if(speedrunTimer && newMessageHandler.active && newMessageHandler.msg.isLineDone())
        {
            messageEvent.Event(this.gameObject);
            this.enabled = false;
        }
    }
    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
    }
    public void LoadData(SaveData data)
    {
        this.speedrunTimer = data.speedrunTimer;
    }
    public void SaveData(ref SaveData data)
    {
        return;
    }
}
