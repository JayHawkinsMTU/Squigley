using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCursor : MonoBehaviour
{
    int index = 0;
    float deadzoneSize = 0.1f;
    bool deadzoneReset = true; //Allows the controller to detect a change from axis instead of repeatedly activating statements.
    
    [SerializeField] AudioSource audioSource;
    [SerializeField]AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        if(Input.GetJoystickNames().Length == 0)
        {
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!(GetComponent<StartButton>().active || GetComponent<QuitButton>().active))
        {
            Time.timeScale = 1;

            if(index == 0)
            {
                GetComponent<StartButton>().available = true;
                GetComponent<QuitButton>().available = false;
            }
            else if(index == 1)
            {
                GetComponent<StartButton>().available = false;
                GetComponent<QuitButton>().available = true;
            }
            if((Input.GetKeyDown("down") || Input.GetKeyDown("s") || (Input.GetAxis("Vertical1") < -deadzoneSize && deadzoneReset)))
            {
                index++;
                if(index != Mathf.Clamp(index, 0, 1))
                {
                    index = Mathf.Clamp(index, 0, 1);
                }
                else
                {
                    audioSource.PlayOneShot(audioClip, 0.5f);
                }
                deadzoneReset = false;
            }
            else if(Input.GetKeyDown("up") || Input.GetKeyDown("w") || (Input.GetAxis("Vertical1") > deadzoneSize && deadzoneReset))
            {
                index--;
                if(index != Mathf.Clamp(index, 0, 1))
                {
                    index = Mathf.Clamp(index, 0, 1);
                }
                else
                {
                    audioSource.PlayOneShot(audioClip, 0.5f);
                }
                deadzoneReset = false;
            }
            else if(!deadzoneReset && Input.GetAxis("Vertical1") < deadzoneSize && Input.GetAxis("Vertical1") > -deadzoneSize)
            {
                deadzoneReset = true;
            }
        }
    }
}
