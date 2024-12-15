using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpWizardPurchase : Decision
{
    [SerializeField] UICoinHandler coinHandler;
    [SerializeField] EnableVisualEvent visual;
    [SerializeField] LockedPortal lockedPortal; //Can be either one.
    NewMessageHandler newMessageHandler;
    [SerializeField] SpriteRenderer spi;
    [SerializeField] Sprite altSprite;
    Sprite ogSprite;
    Wait spriteWait = new Wait(2f);
    [SerializeField] string[] yesMessage;
    [SerializeField] string[] noMessage;
    [SerializeField] string[] alreadyUnlockedMessage;
    [SerializeField] LoopingAnimation purchaseAnimation;
    // Start is called before the first frame update
    void Start()
    {
        newMessageHandler = GetComponent<NewMessageHandler>();
        if(lockedPortal.unlocked)
        {
            newMessageHandler.ReplaceMessage(alreadyUnlockedMessage);
        }
        ogSprite = spi.sprite;
        this.enabled = false; //Shouldn't run update function all the time to save on resources.
    }
    void Update()
    {
        if(spi.sprite == altSprite)
        {
            spriteWait.Iterate();
            if(spriteWait.Complete())
            {
                spriteWait.Reset();
                spi.sprite = ogSprite;
                this.enabled = false;
            }
        }
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        this.enabled = true;
        if(!lockedPortal.unlocked)
        {
            switch(option)
            {
                case 1: //YES
                    if(coinHandler.coinCount >= price)
                    {
                        purchaseAnimation.enabled = false;
                        coinHandler.coinCount -= price;
                        visual.Event(this.gameObject);
                        lockedPortal.Unlock();
                        spi.sprite = altSprite;
                    }
                    else
                    {
                        goto case 2;
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
        else
        {
            newMessageHandler.ReplaceMessage(alreadyUnlockedMessage);
        }
    }
}
