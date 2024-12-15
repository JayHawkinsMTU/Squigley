using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Message
{
    TMP_Text textBox; //The textbox to be modified.
    string[] message = null; //Each element is the next line of text.
    public static string[] globalMessage = {"Welcome, Squigley"}; //This is the message that should be changed by triggers and accessed at checkpoints.
    public static bool newMessage = true; //Used for message notifications.
    public static float textSpeed = 1; //In case I want to make text speed a setting later.
    string activeText = "";
    bool blinkAtEnd = false;
    bool endDotBlink = false; //Determines whether the dot at the end of a message is "0" if true or "_" if false.
    public int lineIndex = 0;
    int charIndex = -1;
    private bool additive = false; //false - line resets on NextLine(). true - new line on NextLine().

    public Message() //Default constructor should never be used.
    {
        Debug.LogError("Default constructor of Message is invoked.");
    }
    public Message(TMP_Text t, string[] m, bool bae) //MessageHandler passes on TMP_Text and message at Start().
    {
        textBox = t;
        message = m;
        blinkAtEnd = bae;
    }
    public Message(TMP_Text t, string[] m, bool bae, bool a) //MessageHandler passes on TMP_Text and message at Start().
    {
        textBox = t;
        message = m;
        blinkAtEnd = bae;
        additive = a;
    }
    public char NextChar() //Advances text forward one character.
    {
        charIndex++;
        activeText += message[lineIndex][charIndex];
        textBox.SetText(activeText);
        return message[lineIndex][charIndex];
    }
    public void NextLine() //Advances text forward one line.
    {
        lineIndex++;
        if(!additive) 
        {
            activeText = "";
            
        } 
        else 
        {
            activeText += "\n";
        }
        charIndex = -1;
        textBox.SetText(activeText);
    }
    public void SkipLine() //Skips to the end of the current line.
    {
        charIndex = message[lineIndex].Length - 1;
        activeText = message[lineIndex];
        textBox.SetText(activeText);
        if(blinkAtEnd) Blink();
    }
    public bool isLineDone() //Returns true if line of message is complete.
    {
        return charIndex >= (message[lineIndex].Length - 1);
    }
    public bool isMessageDone() //Returns true if entire message is complete.
    {
        return (isLineDone() && lineIndex >= (message.Length - 1));
    }
    public void ChangeMessage(string[] m)
    {
        Close();
        message = m;
    }
    public void Blink() //Should be called when isLineDone to indicate to player that the next line is ready.
    {
        if(endDotBlink)
        {
            textBox.SetText(activeText + " 0");
            endDotBlink = false;
        }
        else
        {
            textBox.SetText(activeText + " _");
            endDotBlink = true;
        }
    }
    public void Close()
    {
        lineIndex = 0;
        charIndex = -1;
        activeText = "";
        textBox.SetText(activeText);
    }
}
