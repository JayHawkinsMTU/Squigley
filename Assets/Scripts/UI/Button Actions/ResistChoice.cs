using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResistChoice : Button
{
    [SerializeField] DecisionMatrix decisionMatrix;
    [SerializeField] int choiceID;

    void OnEnable()
    {
        if(!UICoinHandler.admin)
        {
            GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        }
    }
    public override void Activate()
    {
        if(UICoinHandler.admin)
        {
            decisionMatrix.decision.Choose(choiceID);
        }
        else
        {
            UtilityText.primaryInstance.DisplayMsg("ADMIN ACCESS REQUIRED TO MODIFY THEOS", Color.red);
        }
    }
}
