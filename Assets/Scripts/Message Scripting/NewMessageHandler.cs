using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewMessageHandler : MonoBehaviour, Interactable
{
    private float initTextSize = 0;
    private float initBoxSize = 0;
    public static float BLINK_LENGTH = 0.25f;
    [SerializeField] public string [] message = {"Default message"};
    [SerializeField] float charPerSecond = 15;
    [SerializeField] bool globalMessage;
    [SerializeField] TMP_Text textBox;
    AudioSource audioSource;
    [SerializeField] AudioClip voice;
    [SerializeField] float volume = 0.5f;
    [SerializeField] bool blinkAtEnd = true;
    [SerializeField] bool triggerAble = true;
    public MsgInteractTrigger trigger;
    public Message msg;
    private Wait wait;
    private Wait blinkWait;
    public bool active;
    [SerializeField] bool additive = false;
    [SerializeField] MessageEvent[] messageEvents = null;
    [SerializeField] bool skippable = true;
    private int mEIndex = 0;
    public string achievementName = "";
    shopMusic music;

    public void Interact(GameObject p)
    {
        if(active)
        {
            if(msg.isLineDone())
            {
                if(messageEvents.Length != 0)
                {
                    if(messageEvents[mEIndex].index == msg.lineIndex)
                    {
                        messageEvents[mEIndex].Event(p);
                        if(mEIndex < messageEvents.Length - 1)
                            mEIndex++;
                    }
                }
                if(msg.isMessageDone())
                {
                    Close();
                }
                else
                {
                    msg.NextLine();
                }
            }
            else if(skippable)
            {
                msg.SkipLine();
                audioSource.PlayOneShot(voice, volume);
            }
        }
        else
        {
            Open();
        }
    }
    private float SizeModifier()
    {
        return Mathf.Clamp((CameraUpgrades.currentSize - 7f) / 3f, 0f, 5f);
    }
    public void Open()
    {
        if(achievementName != "")
        {
            AchievementManager.GetAchievement(achievementName);
        }
        //Gets initial font size if unassigned.
        if(initTextSize == 0)
        {
            initTextSize = textBox.fontSize; 
        }
        if(initBoxSize == 0)
        {
            initBoxSize = textBox.rectTransform.sizeDelta.x;
        }
        textBox.fontSize = initTextSize + SizeModifier();
        //textBox.rectTransform.sizeDelta = new Vector2(initBoxSize + SizeModifier(), textBox.rectTransform.sizeDelta.y);
        if(globalMessage)
        {
            message = Message.globalMessage;
            Message.newMessage = false;
        }
        msg = new Message(textBox, message, blinkAtEnd, additive);
        active = true;
        enabled = true;
        if(music != null)
        {
            music.StartMusic();
        }
    }
    public void Close()
    {
        msg.Close();
        audioSource.PlayOneShot(voice, volume);
        active = false;
        mEIndex = 0;
        if(music != null)
        {
            music.StopMusic();
        }
    }

    public void ReplaceMessage(string[] m)
    {
        Close();
        Open();
        msg = new Message(textBox, m, blinkAtEnd, additive);
    }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        music = GetComponent<shopMusic>();
        if(trigger == null) trigger = GetComponent<MsgInteractTrigger>();
        wait = new Wait(Message.textSpeed / charPerSecond);
        blinkWait = new Wait(BLINK_LENGTH);
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if(msg.isLineDone())
            {
                if(blinkAtEnd)
                {
                    blinkWait.Iterate();
                    if(blinkWait.Complete())
                    {
                        msg.Blink();
                    }
                }
            }
            else
            {
                wait.Iterate();
                if(wait.Complete())
                {
                    if(msg.NextChar() != ' ')
                    {
                        audioSource.PlayOneShot(voice, volume);
                    }
                    if(msg.isLineDone())
                    {
                        msg.Blink(); //Executed here first to prevent a space from akwardly appearing.
                    }
                }
            }
            if(triggerAble && !trigger.inTrigger)
            {
                Close(); //Closes when player leaves trigger.
            }
        }
    }
}
