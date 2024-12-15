using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuitButton : MonoBehaviour
{
    [SerializeField] TMP_Text ui;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    float timeCountOne = 0f;
    float timeCountTwo = 0f;
    public bool active;
    public bool available = false;
    bool WaitOne(float time)
    {
        timeCountOne += Time.deltaTime;
        if(timeCountOne >= time)
        {
            timeCountOne = 0;
            return true;
        }
        else return false;
    }
    bool WaitTwo(float time)
    {
        timeCountTwo += Time.deltaTime;
        if(timeCountTwo >= time)
        {
            timeCountTwo = 0;
            return true;
        }
        else return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(available)
        {
            ui.color = Color.yellow;
            if(Input.GetKeyDown("z") || Input.GetKeyDown("return") || Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 7"))
            {
                active = true;
                audioSource.PlayOneShot(audioClip, 0.5f);
            }
        }
        else if(!active)
        {
            ui.color = Color.white;
        }
        if(active)
        {
            available = false;
            if(WaitOne(.0725f))
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
            if(WaitTwo(1.5f))
            {
                Application.Quit();
            }
        }
    }
}
