using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Motd : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] string[] motds;
    [SerializeField] float waitTime; //The time to wait before fading in.
    [SerializeField] float outWaitTime;
    [SerializeField] float fadeTime; //The time it takes to fade in.
    private Wait startWait;
    private bool fading = false;
    private bool fadeOut = false;
    // Start is called before the first frame update
    void Start()
    {
        startWait = new Wait(waitTime);
        text = GetComponent<TMP_Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.SetText(motds[Random.Range(0, motds.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        if(!fading)
        {
            startWait.Iterate();
            if(startWait.Complete())
            {
                fading = true;
                startWait = new Wait(outWaitTime);
            }        
        }
        else
        {
            if(!fadeOut)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime / fadeTime);
                if(text.color.a >= 1)
                {
                    fadeOut = true;
                    fading = false;
                }
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime / fadeTime);
            }

        }

    }
}
