using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHandler : MonoBehaviour, IDataPersistence
{
    SpriteRenderer spi;
    Color currentColor = Color.white;
    int playerID;
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        spi.color = currentColor;
        playerID = GetComponent<Movement>().playerID;
    }

    public void ChangeColor(Color c, bool achieve = false)
    {
        spi.color = c;
        currentColor = c;
        // true should be passed when using the color shop
        // or else this achievement is activated upon start
        if(!achieve) return;
        AchievementManager.GetAchievement("NEW_COLOR");
    }
    public void LoadData(SaveData data)
    {
        Start();
        if(playerID == 1)
        {
            this.currentColor = data.player1Color;
        }
        else
        {
            this.currentColor = data.player2Color;
        }
        ChangeColor(currentColor);
    }
    public void SaveData(ref SaveData data)
    {
        if(playerID == 1)
        {
            data.player1Color = this.currentColor;
        }
        else
        {
            data.player2Color = this.currentColor;
        }
    }
}
