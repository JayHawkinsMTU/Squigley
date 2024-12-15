using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait
{
    private float length = 0f; //The amount of time the object will wait for.
    private float timeCount = 0f;

    public Wait(float l)
    {
        length = l;
        timeCount = 0;
    }
    public bool isDone() //Returns true if timeCount exceeds the wait length.
    {
        return timeCount >= length;
    }
    public bool Complete() //Returns true and resets timeCount if done;
    {
        if(isDone())
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Reset()
    {
        timeCount = 0;
    }
    public void SetLength(float l)
    {
        length = l;
    }
    public bool OldWait(float l) //This only exists to make the old method of implementation easier. Do not use in new objects.
    {
        length = l;
        Iterate();
        return Complete();
    }
    public void Iterate()
    {
        if(!isDone())
        {
            timeCount += Time.deltaTime;
        }
    }
}
