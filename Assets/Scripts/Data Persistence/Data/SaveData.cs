using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //[SerializeField] UICoinHandler uiCoinHandler;
    public bool[] unlockedDoors; //True if unlocked. Index corresponds to each door's ID. Set length accordingly when game is finished.
    public bool[] unlockedMessages; //True if unlocked. Index corresponds to each message's ID. Set length accordingly when game is finished. (Achievements)
    public bool[] unlockedSkins; //Index corresponds to skin id according to player 1's skin handler.
    public bool[] unlockedAccessories;
    public int currentCheckpointID; //ID of current checkpoint. 0 if there is no checkpoint.
    public int p1skinID;
    public int p2skinID;
    public int[] accessories = new int[2]; //0 - player 1. 1 - player 2
    public Color player1Color;
    public Color player2Color;
    public bool hasId; //Used for bar.
    public bool parryUnlocked = false;
    public bool admin; // Necessary for plot progression of true ending.

    //Variables grabbed from UICoinHandler
    public int coinCount;
    public int checkPointLives;
    public int totalCoinsCollected;
    public int vasesBroken;
    public int totalDeaths;
    public int messagesTriggered;
    public int blocksBroken;

    //Variables from CameraUpgrades
    public int maxSize;
    public int currentSize;
    public bool smoothCamera;

    //Settings
    public float volume;
    public bool fullScreen;
    public Resolution resolution;
    //State
    public string sceneName;
    //Time
    public bool speedrunTimer = false;
    public int hours;
    public int minutes;
    public int seconds;
    public int milliSeconds;
    //Oblivion Score
    public int highScore;
    //Amount of times game has been reset.
    public bool debug; //Can only be set to true unless save data is reset.
    
    //Bindings
    public string jump1 = GameInput.DEF_JUMP1;
    public string jump2 = GameInput.DEF_JUMP2;
    public string jump3 = GameInput.DEF_JUMP3;
    public string moveleft1 = GameInput.DEF_MOVELEFT1;
    public string moveleft2 = GameInput.DEF_MOVELEFT2;
    public string moveright1 = GameInput.DEF_MOVERIGHT1;
    public string moveright2 = GameInput.DEF_MOVERIGHT2;
    public string crouch1 = GameInput.DEF_CROUCH1;
    public string crouch2 = GameInput.DEF_CROUCH2;
    public string interact1 = GameInput.DEF_INTER1;
    public string interact2 = GameInput.DEF_INTER2; 

    public SaveData() //Constructor
    {
        unlockedDoors = new bool[10];
        unlockedMessages = new bool[25];
        unlockedSkins = new bool[15];
        unlockedAccessories = new bool[15];
        unlockedSkins[0] = true;
        currentCheckpointID = 0;
        p1skinID = 0;
        p2skinID = 9;
        accessories = new int[2];
        player1Color = Color.white;
        player2Color = new Color(1, 40/255, 40/255);
        hasId = false;
        coinCount = 0;
        checkPointLives = 0;
        totalCoinsCollected = 0;
        vasesBroken = 0;
        totalDeaths = 0;
        messagesTriggered = 0;
        blocksBroken = 0;
        maxSize = 9;
        currentSize = 8;
        smoothCamera = false;
        volume = 1;
        fullScreen = false;
        resolution.width = 1920;
        resolution.height = 1080;
        sceneName = "Overworld";
        speedrunTimer = false;
        hours = 0;
        minutes = 0;
        seconds = 0;
        milliSeconds = 0;
        highScore = 1000;
        Iteration.iteration++;
        debug = false;
        admin = false;
        parryUnlocked = false;

        //Bindings
        jump1 = GameInput.DEF_JUMP1;
        jump2 = GameInput.DEF_JUMP2;
        jump3 = GameInput.DEF_JUMP3;
        moveleft1 = GameInput.DEF_MOVELEFT1;
        moveleft2 = GameInput.DEF_MOVELEFT2;
        moveright1 = GameInput.DEF_MOVERIGHT1;
        moveright2 = GameInput.DEF_MOVERIGHT2;
        crouch1 = GameInput.DEF_CROUCH1;
        crouch2 = GameInput.DEF_CROUCH2;
        interact1 = GameInput.DEF_INTER1;
        interact2 = GameInput.DEF_INTER2; 
    }
}
