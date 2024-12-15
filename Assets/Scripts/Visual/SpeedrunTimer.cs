using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour, IDataPersistence
{
    [SerializeField] TimeData timeData;
    TMP_Text timeDisplay;
    bool active;

    string FormatTime(int digits, int units)
    {
        string display = units.ToString();
        switch(digits)
        {
            case 3:
                if(units < 100)
                {
                    display = "0" + display;
                }
                goto case 2;
            case 2:
                if(units < 10)
                {
                    display = "0" + display;
                }
                break;
        }
        return display;
    }
    void UpdateDisplay()
    {        
        string formattedDisplay = timeData.hours + ":" + FormatTime(2, timeData.minutes) + ":" + FormatTime(2, timeData.seconds) + ":" + FormatTime(3, timeData.milliSeconds);
        timeDisplay.SetText(formattedDisplay);
    }
    void Awake()
    {
        timeDisplay = GetComponent<TMP_Text>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    public void LoadData(SaveData data)
    {
        active = data.speedrunTimer;
        timeDisplay = GetComponent<TMP_Text>();
        timeDisplay.enabled = active;
    }
    public void SaveData(ref SaveData data)
    {
        return;
    }
}
