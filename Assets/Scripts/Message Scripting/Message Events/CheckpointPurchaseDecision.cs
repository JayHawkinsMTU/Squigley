using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPurchaseDecision : Decision
{
    [SerializeField] Checkpoint checkpoint;
    [SerializeField] UICoinHandler coinHandler;
    NewMessageHandler newMessageHandler;
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;

    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
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
                    checkpoint.Activate(0);
                }
                else if(coinHandler.coinCount >= price)
                {
                    coinHandler.coinCount -= price;
                    checkpoint.Activate(0);
                    AchievementManager.GetAchievement("CHECKPOINT_GET");
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
                Debug.Log("Invalid choice, should be 1 or 2.");
                break;
        }
    }
}
