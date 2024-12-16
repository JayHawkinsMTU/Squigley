using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantAchievementOnStart : MonoBehaviour
{
    public string achievementName;
    // Start is called before the first frame update
    void Start()
    {
        AchievementManager.GetAchievement(achievementName);   
    }
}
