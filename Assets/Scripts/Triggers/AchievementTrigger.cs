using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTrigger : MonoBehaviour
{
    [SerializeField] string achievementName;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            AchievementManager.GetAchievement(achievementName);
        }
    }
}
