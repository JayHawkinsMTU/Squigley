using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerLogic : MonoBehaviour
{
    public GameObject player;
    NewMessageHandler handler;
    [SerializeField] int keyID;
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
        if(!done && handler.active && player.GetComponent<SkinHandler>().skinID == keyID)
        {
            done = true;
            handler.msg.ChangeMessage(newMessage);
            door.Unlock();
        }
    }
}
