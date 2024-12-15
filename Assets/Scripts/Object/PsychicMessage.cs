using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicMessage : MonoBehaviour
{
    public GameObject eventSystem;
    public NewMessageHandler message;
    bool swapped = false;


    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
        message = GetComponent<NewMessageHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(message.active)
        {
            if(!swapped)
            {
                swapped = true;
                switch(Random.Range(0,5))
                {
                    case 0:
                        message.msg.ChangeMessage(new string[]{"Found quite the pennies on your travels, eh?", eventSystem.GetComponent<UICoinHandler>().totalCoinsCollected + " coins to be exact."});
                        break;
                    case 1:
                        message.msg.ChangeMessage(new string[]{"Quite the clutz, aren't you?", "You've broken quite a few vases in your time.", eventSystem.GetComponent<UICoinHandler>().vasesBroken + " to be exact.", "Like a bull through a china shop, you are."});
                        break;
                    case 2:
                        message.msg.ChangeMessage(new string[]{"You ever feel like no one understands your pain?", "I certainly can't claim to.", "I'll never know what its like to have died " + eventSystem.GetComponent<UICoinHandler>().totalDeaths + " times."});
                        break;
                    case 3:
                        message.msg.ChangeMessage(new string[]{"Do you consider yourself something of a scavenger?", "After all, you've found quite a few secrets.", eventSystem.GetComponent<UICoinHandler>().messagesTriggered + " to be exact."});
                        break;
                    case 4:
                        message.msg.ChangeMessage(new string[]{"You're destructive aren't you?", "You've destroyed " + eventSystem.GetComponent<UICoinHandler>().blocksBroken + " blocks so far.", "Perhaps you don't know your own strength."});
                        break;
                }
            }
        }
        else if(swapped)
        {
            swapped = false;
        }
    }
}
