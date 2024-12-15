using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceButton : Button
{
    [SerializeField] DecisionMatrix decisionMatrix;
    [SerializeField] int choiceID;
    public override void Activate()
    {
        decisionMatrix.decision.Choose(choiceID);
    }
}
