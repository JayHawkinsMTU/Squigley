using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviour, IDataPersistence
{
    public SpriteRenderer spi;
    public Sprite [] skins;
    public int skinID;
    public bool[] unlockedSkins = new bool[13];
    //[SerializeField] bool debugSkin = false;
    private int playerID;

    public void UpdateSkin(int id)
    {
        unlockedSkins[id] = true; //Permentantly unlocks skin;
        if(AllSkinsUnlocked())
        {
            AchievementManager.GetAchievement("ALL_SKINS");
        }
        skinID = Mathf.Clamp(id, 0, skins.Length);
        spi.sprite = skins[skinID];
        skinID = id;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerID = GetComponent<Movement>().playerID;
        UpdateSkin(skinID);
    }

    bool AllSkinsUnlocked()
    {
        foreach(bool b in unlockedSkins)
        {
            if(!b)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    /*void Update()
    {
        if(debugSkin)
        {
            UpdateSkin(skinID);
        }
    }*/
    public void LoadData(SaveData data)
    {
        playerID = GetComponent<Movement>().playerID;
        if(playerID == 1)
            this.skinID = data.p1skinID;
        else
            this.skinID = data.p2skinID;
        this.unlockedSkins = data.unlockedSkins;
        UpdateSkin(skinID);
    }
    public void SaveData(ref SaveData data)
    {
        if(playerID == 1)
        {
            data.p1skinID = this.skinID;
            data.unlockedSkins = this.unlockedSkins;
        }
        else
            data.p2skinID = this.skinID;
    }
}
