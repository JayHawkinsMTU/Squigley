using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinBuyLogicHandler : MonoBehaviour
{
    [SerializeField] int purchaseIndex;
    public GameObject eventSystem;
    public GameObject player;
    [SerializeField] int price;
    [SerializeField] string [] replaceMessage = {"You're broke as shit, ", " get out of here."};
    [SerializeField] int skinID;
    NewMessageHandler handler;
    string [] ogMsg;
    public AudioSource audioSource;
    public AudioClip checkpointGet;
    bool checkpointActivated = false;
    bool replaceActivated = false;
    void Start()
    {
        player = GameObject.Find("Player");
        eventSystem = GameObject.Find("EventSystem");
        //Debug.Log(ogMsg[purchaseIndex]);
        handler = GetComponent<NewMessageHandler>();
        ogMsg = handler.message;
        audioSource = GetComponent<AudioSource>();
    }

    void Buy()
    {
        audioSource.PlayOneShot(checkpointGet, 0.5f);
        checkpointActivated = true;
        replaceActivated = true;
        player.GetComponent<SkinHandler>().UpdateSkin(skinID);
        eventSystem.GetComponent<UICoinHandler>().coinCount -= price;
    }
    void Update()
    {
        if(handler.active)
        {
            if(handler.msg.lineIndex == purchaseIndex)
            {
                if(!checkpointActivated)
                {
                    if(eventSystem.GetComponent<UICoinHandler>().coinCount >= price)
                    {
                        Buy();
                        /*
                        GetComponent<SingleMessageHandler>().message[purchaseIndex] = ogMsg;
                        GetComponent<SingleMessageHandler>().messageIndex = 0;
                        GetComponent<SingleMessageHandler>().text.SetText("");
                        replaceActivated = true;
                        player.GetComponent<SkinHandler>().UpdateSkin(skinID);
                        audioSource.PlayOneShot(checkpointGet, 0.5f);
                        checkpointActivated = true;
                        eventSystem.GetComponent<UICoinHandler>().coinCount -= price;*/
                    }
                    else 
                    {
                        handler.msg.ChangeMessage(replaceMessage);
                    }
                }
                if(!replaceActivated)
                {
                    replaceActivated = true;
                }
            }
            else
            {
                replaceActivated = false;
                checkpointActivated = false;
            }
        }
        else if(replaceActivated)
        {
            handler.msg.ChangeMessage(ogMsg); //Sets the message back to normal.
        }
    }
}
