using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData : MonoBehaviour, IDataPersistence
{
    //Modes
    const int MILLISECOND = 0;
    const int SECOND = 1;
    const int MINUTE = 2;
    //Data
    public int hours;
    public int minutes;
    public int seconds;
    public int milliSeconds;
    Pause pauseScript;
    private bool running;
    [SerializeField] bool loadAndSave = true;

    void ConsolidateTime(int mode)
    {
        switch(mode)
        {
            case MILLISECOND:
                if(milliSeconds >= 1000)
                {
                    int deltaSeconds = (int) milliSeconds / 1000; //Should always be one except for instances of an extreme stutter greater than 1s.
                    seconds += deltaSeconds;
                    milliSeconds -= deltaSeconds * 1000;
                    ConsolidateTime(SECOND);
                }
                break;
            case SECOND:
                if(seconds >= 60)
                {
                    minutes += 1;
                    seconds -= 60;
                    ConsolidateTime(MINUTE);
                }
                break;
            case MINUTE:
                if(minutes >= 60)
                {
                    hours += 1;
                    minutes -= 60;
                }
                break;
        }
        
        
    }
    void Tick()
    {
        if(!pauseScript.paused)
        {
            milliSeconds += (int) (Time.deltaTime * 1000);
            ConsolidateTime(MILLISECOND);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseScript = GetComponent<Pause>(); //Must be on the same game object (likely EventSystem) as Pause.cs
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(running)
            Tick();
    }
    public void EndTime() //Used at the end of the game and at load time so speedrunners can get an accurate time reading.
    {
        running = false;
    }
    public void LoadData(SaveData data)
    {
        if(!loadAndSave) return;
        this.hours = data.hours;
        this.minutes = data.minutes;
        this.seconds = data.seconds;
        this.milliSeconds = data.milliSeconds;
    }
    public void SaveData(ref SaveData data)
    {
        if(!loadAndSave) return;
        data.hours = this.hours;
        data.minutes = this.minutes;
        data.seconds = this.seconds;
        data.milliSeconds = this.milliSeconds;
    }
}
