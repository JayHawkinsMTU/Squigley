using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoDecision : Decision
{
    public NewMessageHandler[] branches;
    public MsgInteractTrigger msgInteractTrigger;
    public int goodbyeMessage = -1; // Index of message that exits conversation
    public override void Choose(int option)
    {
        // Start next message according to option
        branches[option].gameObject.SetActive(true);
        msgInteractTrigger.interactable = branches[option];
        if(option != goodbyeMessage)
            branches[option].Open();
        
        // Close previous message
        GetComponent<NewMessageHandler>().Close();
        decisionMatrix.Close();
        this.gameObject.SetActive(false);

    }
}
