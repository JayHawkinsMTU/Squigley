using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunTimerButton : Button
{
    [SerializeField] GameObject tickMark;
    public bool visible = false;
    void Awake()
    {
        UpdateTickmark();
    }
    public void UpdateTickmark()
    {
        tickMark.SetActive(visible);
    }

    public override void Activate()
    {
        visible = !visible;
        UpdateTickmark();
    }
}
