using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPurchaseDecision : Decision
{
    [SerializeField] UICoinHandler coinHandler;
    [SerializeField] int skinID;
    NewMessageHandler newMessageHandler;
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;
    SkinHandler skinHandler;

    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
    }
    public override void Event(GameObject p)
    {
        skinHandler = p.GetComponent<SkinHandler>();
        OpenMatrix(title);
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        switch(option)
        {
            case 1: //YES
                if(robPlayer)
                {
                    coinHandler.coinCount = 0;
                    skinHandler.UpdateSkin(skinID);
                }
                else if(coinHandler.coinCount >= price)
                {
                    coinHandler.coinCount -= price;
                    skinHandler.UpdateSkin(skinID);
                }
                else
                {
                    goto case 2; //When you're broke.
                }
                if(yesMessage.Length != 0)
                    newMessageHandler.ReplaceMessage(yesMessage);
                break;
            case 2:
                if(noMessage.Length != 0)
                    newMessageHandler.ReplaceMessage(noMessage);
                break;
            default:
                Debug.Log("Invalid choice, should be 1 or 2.");
                break;
        }
    }
}
