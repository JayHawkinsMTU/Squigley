using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPurchaseDecision : Decision
{
    [SerializeField] LockedDoor door;
    [SerializeField] UICoinHandler coinHandler;
    NewMessageHandler newMessageHandler;
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;
    [SerializeField] string[] alreadyUnlockedMessage;

    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        if(door.unlocked)
        {
            newMessageHandler.ReplaceMessage(alreadyUnlockedMessage);
        }
        else
        {
            switch(option)
            {
                case 1: //YES
                    if(robPlayer)
                    {
                        coinHandler.coinCount = 0;
                        door.Unlock();
                    }
                    else if(coinHandler.coinCount >= price)
                    {
                        coinHandler.coinCount -= price;
                        door.Unlock();
                    }
                    else
                    {
                        goto case 2; //When you're broke.
                    }
                    newMessageHandler.ReplaceMessage(yesMessage);
                    break;
                case 2:
                    newMessageHandler.ReplaceMessage(noMessage);
                    break;
                default:
                    Debug.Log("Invalid choice, should be 1 or 2. Are you using Y/N matrix?");
                    break;
            }
        }
    }
}
