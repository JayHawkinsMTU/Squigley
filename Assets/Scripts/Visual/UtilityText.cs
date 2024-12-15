using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UtilityText : MonoBehaviour
{
    public static UtilityText primaryInstance;
    const float MSG_LENGTH = 5f;
    TMP_Text text;
    Wait wait = new Wait(MSG_LENGTH);

    public void DisplayMsg(string msg, Color color, float length = MSG_LENGTH)
    {
        text.enabled = true;
        text.SetText(msg);
        text.color = color;
        wait = new Wait(length);
        this.enabled = true;
    }
    public void CloseMsg()
    {
        text.SetText("");
        text.enabled = false;
        this.enabled = false;
    }
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        primaryInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        wait.Iterate();
        if(wait.Complete())
        {
            CloseMsg();
        }
    }
}
