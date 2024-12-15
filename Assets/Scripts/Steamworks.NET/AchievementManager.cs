using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementManager
{
    public static void GetAchievement(string achName)
    {
        if(!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement(achName);
        SteamUserStats.StoreStats();
    }
}
