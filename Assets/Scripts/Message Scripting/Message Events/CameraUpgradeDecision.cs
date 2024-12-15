using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpgradeDecision : Decision
{
    [SerializeField] CameraUpgrades cameraUpgrades;
    [SerializeField] UICoinHandler coinHandler;
    private int cameraLevel;
    private int maxLevel = 16;
    private int[] upgradePrices = {20, 40, 80, 160, 320}; //After level 5, every price should be 320.
    NewMessageHandler newMessageHandler;
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;
    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
    }
    public override void Event(GameObject p)
    {
        cameraLevel = cameraUpgrades.maxSize - 9;
        price = upgradePrices[Mathf.Clamp(cameraLevel, 0, upgradePrices.Length - 1)];
        OpenMatrix(title + ": " + price.ToString());
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        switch(option)
        {
            case 1: //YES
                if(coinHandler.coinCount >= price && cameraUpgrades.maxSize < maxLevel)
                {
                    coinHandler.coinCount -= price;
                    cameraUpgrades.maxSize++;
                    cameraUpgrades.ZoomOut();
                    cameraLevel++;
                    newMessageHandler.ReplaceMessage(yesMessage);
                }
                else
                {
                    goto case 2;
                }
                break;
            case 2: //NO or not enough money.
                newMessageHandler.ReplaceMessage(noMessage);
                break;
            default:
                break;
        }
    }

}
