using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalMessageConstructor : MonoBehaviour, IDataPersistence
{
    
    //THESE CONSTANTS ARE ESTIMATES, VERIFY BEFORE LAUNCH!!!
    private const int MAX_NUM_SKINS = 13;
    private const int MAX_NUM_MESSAGES = 12;
    private const int MAX_NUM_DOORS = 9;
    private const int COIN_PAR = 10000; //Value for rich accolade.
    private const int HOUR_PAR = 1; //Hour value for speedy accolade.
    private const int MINUTE_PAR = 10; //Minute value for the speedrunner accolade.
    private const int DESTRUCTION_PAR = 1000; //Value of destroyed vases and blocks for destructive accolade.
    private const int DEATH_PAR = 2500; //Value for persistent accolade.
    
    //Data to load
    private bool[] unlockedDoors;
    private bool[] unlockedMessages;
    private bool[] unlockedSkins;
    private int iteration = 0;
    private int hours = 0;
    private int minutes = 0;
    private int seconds = 0;
    private int milliSeconds = 0;
    private int totalCoinsCollected;
    private int vasesBroken;
    private int totalDeaths;
    private int blocksBroken;
    private int highScore; //Oblivion

    //Local values
    private int completionPoints = MAX_NUM_DOORS + MAX_NUM_MESSAGES + MAX_NUM_SKINS;
    private int earnedPoints = 0;
    private int totalGameScore = 0;
    private string accolades = ""; //Add accolades in order.
    private NewMessageHandler newMessageHandler;
    private string[] replaceMessage = {
        ">kill squigley.cns",
        "Ending CONCIOUSNESS NEURAL SYSTEM process...",
        "Reading data...",
        "\"Squigley\" ended.",
        "********* SUMMARY *********",
        "Iteration: 0",
        "Accolades: Rich, Speedy, Destructive, Persistent",
        "Mood: Melancholic",
        "Time: 4:00:00",
        "Oblivion Score: 1,000",
        "Overall Score: 1,000,000",
        "Completion: 24%",
        ">exec credit_sequence.exe",
        "Loading..."};


    void ReplaceMessage()
    {
        iteration = Iteration.iteration;
        newMessageHandler = GetComponent<NewMessageHandler>();
        replaceMessage = newMessageHandler.message;
        AssignMood();
        CalculateCompletion();
        AddAccolades();
        AssignTime();
        totalGameScore += highScore;
        totalGameScore += totalCoinsCollected;
        totalGameScore -= totalDeaths * 5;
        replaceMessage[5] = "Iteration: " + iteration.ToString();
        replaceMessage[9] = "Oblivion Score: " + highScore.ToString("###,###,###");
        replaceMessage[10] = "Overall Score: " + totalGameScore.ToString("###,###,###");
        newMessageHandler.message = replaceMessage;
    }
    void AssignTime()
    {
        string formattedtime = hours.ToString() + ":" + minutes.ToString("0#") + ":" + seconds.ToString("0#") + ":" + milliSeconds.ToString("00#");
        replaceMessage[8] = "Time: " + formattedtime;
    }
    void AddAccolades()
    {
        if(hours < HOUR_PAR)
        {
            string newAcc = "SPEEDY ";
            totalGameScore += 1000;
            if(minutes < MINUTE_PAR)
            {
                newAcc = "SPEEDRUNNER ";
                totalGameScore += 10000;
            }
            accolades += newAcc;
        }
        if(totalCoinsCollected >= COIN_PAR)
        {
            accolades += "RICH ";
            totalGameScore += 2000;
        }
        if(vasesBroken + blocksBroken >= DESTRUCTION_PAR)
        {
            accolades += "DESTRUCTIVE ";
            totalGameScore += 500;
        }
        if(totalDeaths >= DEATH_PAR)
        {
            accolades += "PERSISTENT ";
            totalGameScore += 100;
        }
        else if(totalDeaths == 0)
        {
            accolades += "DEATHLESS ";
            totalGameScore += 10000;
        }
        replaceMessage[6] = "Accolades: " + accolades;
    }
    void CalculateCompletion()
    {
        int numOfUnlockedDoors = 0;
        foreach(bool e in unlockedDoors)
        {
            if(e)
            {
                numOfUnlockedDoors++;
                totalGameScore += 100;
            }
        }
        int numOfUnlockedMessages = 0;
        foreach(bool e in unlockedMessages)
        {
            if(e)
            {
                numOfUnlockedMessages++;
                totalGameScore += 50;
            }
        }
        int numOfUnlockedSkins = 0;
        foreach(bool e in unlockedSkins)
        {
            if(e)
            {
                numOfUnlockedSkins++;
                totalGameScore += 100;
            }
        }
        earnedPoints = numOfUnlockedDoors + numOfUnlockedMessages + numOfUnlockedSkins;
        float completion = 100 * ((float) (earnedPoints) / (float) (completionPoints));
        replaceMessage[11] = "Completion: " + completion.ToString("###.##") + "%";
        if(completion >= 100)
        {
            AchievementManager.GetAchievement("COMPLETION_ENDING_1");
        }
    }
    void AssignMood()
    {
        string mood = "Clueless";
        if(unlockedMessages[2])
        {
            mood = "Curious";
            totalGameScore += 50;
        }
        if(unlockedMessages[3])
        {
            mood = "Gullible";
            totalGameScore += 75;

        }
        if(unlockedMessages[5])
        {
            mood = "Thrillseeking";
            totalGameScore += 500;
        }
        if(unlockedMessages[6])
        {
            mood = "Greedy";
            totalGameScore += 100;
        }
        if(unlockedMessages[7]) //Bar
        {
            mood = "Sorrowful";
            totalGameScore += 100;
        }
        if(unlockedMessages[9])
        {
            mood = "Careless";
            totalGameScore += 150;
        }
        if(unlockedMessages[10]) //Random ass big coin
        {
            mood = "Insane";
            totalGameScore += 100;
        }
        if(unlockedMessages[12])
        {
            mood = "Stupid";
            totalGameScore += 500;
        }
        replaceMessage[7] = "Mood: " + mood;

    }
    public void LoadData(SaveData data)
    {
        this.unlockedDoors = data.unlockedDoors;
        this.unlockedMessages = data.unlockedMessages;
        this.unlockedSkins = data.unlockedSkins;
        this.hours = data.hours;
        this.minutes = data.minutes;
        this.seconds = data.seconds;
        this.milliSeconds = data.milliSeconds;
        this.totalCoinsCollected = data.totalCoinsCollected;
        this.vasesBroken = data.vasesBroken;
        this.totalDeaths = data.totalDeaths;
        this.blocksBroken = data.blocksBroken;
        this.highScore = data.highScore;
        ReplaceMessage();
    }
    public void SaveData(ref SaveData data)
    {
        return;
    }
}
