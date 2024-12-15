using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuyLogicHandler : MonoBehaviour
{
    [SerializeField] int purchaseIndex;
    public GameObject eventSystem;
    public GameObject door;
    [SerializeField] int price;
    [SerializeField] string[] replaceMessage = {"You're broke as shit, get out of here."};
    [SerializeField] string[] activatedMessage = {"That key was the only thing I had.", "This business venture had a very weak foundation."};
    public AudioSource audioSource;
    public AudioClip checkpointGet;
    bool doorActivated = false;
    bool replaceActivated = false;
    NewMessageHandler handler;
    void Start()
    {
        handler = GetComponent<NewMessageHandler>();
    }

    
    void Update()
    {
        if(handler.active)
        {
            if(handler.msg.lineIndex == purchaseIndex)
            {
                if(eventSystem.GetComponent<UICoinHandler>().coinCount >= price && !doorActivated)
                {
                    door.GetComponent<LockedDoor>().Unlock();
                    audioSource.PlayOneShot(checkpointGet, 0.5f);
                    doorActivated = true;
                    eventSystem.GetComponent<UICoinHandler>().coinCount -= price;
                }
                else if(!doorActivated)
                {
                    handler.msg.ChangeMessage(replaceMessage);
                }
                if(!replaceActivated)
                {
                    replaceActivated = true;
                }
            }
            else
            {
                replaceActivated = false;
                doorActivated = false;
            }
            if(!door.GetComponent<LockedDoor>().door.GetComponent<BoxCollider2D>().enabled && handler.msg.lineIndex == 0)
            {
                handler.msg.ChangeMessage(activatedMessage);
                enabled = false;
            }
        }
    }
}
