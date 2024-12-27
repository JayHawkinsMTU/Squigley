using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerLogic : MonoBehaviour
{
    public GameObject player;
    NewMessageHandler handler;
    [SerializeField] int keyID;
    [SerializeField] int altKeyID = -1; // For when one other skin can be used as key
    [SerializeField] LockedDoor door;
    [SerializeField] string[] newMessage;

    public void Check()
    {
        player = GameObject.Find("Player");
        handler = GetComponent<NewMessageHandler>();
        int skinID = player.GetComponent<SkinHandler>().skinID;
        if(door.unlocked || handler.active && skinID == keyID || skinID == altKeyID)
        {
            handler.msg.ChangeMessage(newMessage);
            door.Unlock();
        }
    }
}
