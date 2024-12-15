using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosetDecision : Decision
{
    [SerializeField] SkinMatrix skinMatrix;
    GameObject player;
    public override void Event(GameObject p)
    {
        OpenMatrix(title);
        player = p;
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        switch(option)
        {
            case 1: //YES
                skinMatrix.gameObject.transform.parent.gameObject.SetActive(true);
                skinMatrix.Open(player);
                break;
            default:
                break;
        }
    }
}
