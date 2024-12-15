using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleMessageHandler : MonoBehaviour
{
    //This system is incredibly flawed. Redesign it before launch.
    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip voice;
    public TMP_Text text;

    int timeCount = 0;
    [SerializeField] int textPerSecond = 10;
    int textSpeed;
    public int messageIndex = 0;

    bool available = false;
    public bool textBoxActive = false;
    bool endDotBlink = false;
    bool messageDone = false;
    [SerializeField] public string[] message = {"Welcome"};
    public string activeText = "";
    public int messageArray = 0;
    float volume = 0.25f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log(available);
            available = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        messageArray = 0;
        messageIndex = 0;
        
        available = false;
        textBoxActive = false;
        messageDone = false;
        activeText = "";

        text.SetText(activeText);
    }
    // Start is called before the first frame update
    void Start()
    {
        textSpeed = (int)((1f/Time.deltaTime) / textPerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        if(available && (Input.GetKeyDown("z") || Input.GetKeyDown("return") || Input.GetKeyDown("joystick button 2")))
        {
            audioSource.PlayOneShot(click, 0.5f);
            textBoxActive = true;
            textPerSecond *= 5;
            volume /= 2;
            
            if(messageDone)
            {
                messageDone = false;
                
                if(messageIndex < message.Length)
                {
                    activeText = "";
                    text.SetText(activeText);
                    messageArray++;
                }
                if(messageArray >= message.Length)
                {
                    messageArray = 0;
                    textBoxActive = false;
                    activeText = "";
                    text.SetText(activeText);
                }
            }
            
        }
        else if(Input.GetKeyUp("z") || Input.GetKeyUp("return") || Input.GetKeyUp("joystick button 2"))
        {
            textPerSecond /= 5;
            volume *= 2;
        }
        if(textBoxActive)
        {
            textSpeed = (int)((1f/Time.deltaTime) / textPerSecond);
            timeCount++;
            
            if((timeCount >= textSpeed) & (!messageDone))
            {
                activeText += message[messageArray][messageIndex];
                if(messageIndex < message[messageArray].Length - 1)
                {
                    messageIndex++;
                    text.SetText(activeText);
                    audioSource.PlayOneShot(voice, volume);
                }
                else
                {
                    messageDone = true;
                    //Debug.Log("Message Complete");
                    messageIndex = 0;
                }
                timeCount = 0;
            }
            if(messageDone)
            {
                if(timeCount >= textSpeed)
                {
                    if(endDotBlink)
                    {
                        text.SetText(activeText + " 0");
                        endDotBlink = false;
                    }
                    else
                    {
                        text.SetText(activeText + " _");
                        endDotBlink = true;
                    }
                    timeCount = 0;
                }
            }
            else
            {
                endDotBlink = false;
                
            }
        }
    }
}
