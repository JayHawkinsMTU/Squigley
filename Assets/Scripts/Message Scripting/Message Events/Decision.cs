using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Decision : MessageEvent
{
    public DecisionMatrix decisionMatrix;
    public string title = "title";
    public bool robPlayer = false;
    public int price = 0;
    public override void Event(GameObject p)
    {
        OpenMatrix(title);
    }
    public void OpenMatrix(string title)
    {
        decisionMatrix.gameObject.transform.parent.gameObject.SetActive(true);
        if(title != "")
        {
            decisionMatrix.title.SetText(title);
        }
        decisionMatrix.decision = this;
        decisionMatrix.Open();
    }
    public virtual void Choose(int option)
    {
        decisionMatrix.Close();
        Debug.Log(option);
    }
}
