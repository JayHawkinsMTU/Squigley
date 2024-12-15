using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPurchaseDecision : Decision
{
    [SerializeField] UICoinHandler coinHandler;
    NewMessageHandler newMessageHandler;
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;
    [SerializeField] GameObject drinkPrefab;
    [SerializeField] Vector3 location;

    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
    }
    public override void Event(GameObject p)
    {
        if(GetComponent<BartenderLogic>().hasId)
        {
            base.OpenMatrix(title);
        }
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        switch(option)
        {
            case 1: //YES
                if(coinHandler.coinCount >= price)
                {
                    coinHandler.coinCount -= price;
                    Instantiate(drinkPrefab, transform.position + location, Quaternion.identity, transform);
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
