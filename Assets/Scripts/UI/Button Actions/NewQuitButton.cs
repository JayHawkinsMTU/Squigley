using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewQuitButton : Button
{
    TMP_Text ui;
    Wait waitOne = new Wait(0.0725f);
    Wait waitTwo = new Wait(1.5f);
    private bool active;

    public override void Activate()
    {
        active = true;
        transform.parent.parent.GetComponent<ButtonMatrix>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            waitOne.Iterate();
            waitTwo.Iterate();
            if(waitOne.Complete())
            {
                if(ui.color == Color.gray || ui.color == Color.yellow)
                {
                    ui.color = Color.white;
                }
                else if(ui.color == Color.white)
                {
                    ui.color = Color.gray;
                }
            }
            if(waitTwo.Complete())
            {
                Application.Quit();
            }
        }
    }
}
