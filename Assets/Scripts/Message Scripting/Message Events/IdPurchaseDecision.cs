using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdPurchaseDecision : Decision
{
    [SerializeField] BartenderLogic bartender;
    [SerializeField] UICoinHandler coinHandler;
    [SerializeField] NewMessageHandler newMessageHandler;
    [SerializeField] GameObject id;
    public string[] yesMessage;
    public string[] noMessage;
    public string[] brokeMessage;
    
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        if(Random.Range(0f, 1f) >= 0.5)
        {
            robPlayer = false;
        }
        else
        {
            robPlayer = true;
        }
        switch(option)
        {
            case 1: //YES
                
                if(coinHandler.coinCount >= price)
                {
                    coinHandler.coinCount -= price;
                    if(robPlayer)
                    {
                        coinHandler.coinCount = 0;
                    }
                    bartender.hasId = true;
                    newMessageHandler.ReplaceMessage(yesMessage);
                    id.SetActive(true);
                }
                else
                {
                    newMessageHandler.ReplaceMessage(brokeMessage);
                }
                break;
            case 2:
                newMessageHandler.ReplaceMessage(noMessage);
                break;
            default:
                Debug.Log("Invalid choice, should be 1 or 2.");
                break;
        }
    }
}
