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
    bool done;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        handler = GetComponent<NewMessageHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        int skinID = player.GetComponent<SkinHandler>().skinID;
        if(!done && handler.active && skinID == keyID || skinID == altKeyID)
        {
            done = true;
            handler.msg.ChangeMessage(newMessage);
            door.Unlock();
        }
    }
}
