using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSwitchLogic : MonoBehaviour
{
    [SerializeField] int purchaseIndex;
    public GameObject player;
    public GameObject eventSystem;
    //public GameObject checkpoint;
    [SerializeField] int price;
    [SerializeField] string replaceMessage = "You're broke as shit, get out of here.";
    public AudioSource audioSource;
    public AudioClip checkpointGet;
    bool checkpointActivated = false;
    bool replaceActivated = false;
    [SerializeField] int skinID = 0;
    void Start()
    {
        player = GameObject.Find("Player");
        eventSystem = GameObject.Find("EventSystem");
    }

    
    void Update()
    {
        if(GetComponent<SingleMessageHandler>().textBoxActive && GetComponent<SingleMessageHandler>().messageArray == purchaseIndex)
        {
            if(eventSystem.GetComponent<UICoinHandler>().coinCount >= price && !checkpointActivated)
            {
                player.GetComponent<SkinHandler>().UpdateSkin(skinID);
                eventSystem.GetComponent<UICoinHandler>().coinCount -= price;
                checkpointActivated = true;
            }
            else if(!checkpointActivated)
            {
                
                GetComponent<SingleMessageHandler>().message[purchaseIndex] = replaceMessage;
                
            }
            if(!replaceActivated)
            {
                replaceActivated = true;
                GetComponent<SingleMessageHandler>().messageIndex = 0;
                GetComponent<SingleMessageHandler>().text.SetText("");
            }
        }
        else
        {
            replaceActivated = false;
            checkpointActivated = false;
        }
    }
}

