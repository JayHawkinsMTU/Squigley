using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkinButton : Button
{
    [SerializeField] private int skinID;
    private bool unlocked;
    [SerializeField] Image skinDisplay;
    [SerializeField] Sprite skinSprite;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] GameObject priceDisplay;
    [SerializeField] int price = 100;
    public SkinHandler skinHandler;
    UICoinHandler coinHandler;
    [SerializeField] AudioClip equipSound;
    [SerializeField] AudioClip purchaseSound;
    [SerializeField] AudioClip cantUnlockSound;

    void Start()
    {
        skinHandler = transform.parent.parent.gameObject.GetComponent<SkinMatrix>().player.GetComponent<SkinHandler>();
        unlocked = skinHandler.unlockedSkins[skinID];
        coinHandler = GameObject.Find("EventSystem").GetComponent<UICoinHandler>();
        if(!unlocked)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }
    void Lock()
    {
        skinDisplay.sprite = lockedSprite;
        priceDisplay.transform.parent.gameObject.SetActive(true);
        priceDisplay.GetComponent<TMP_Text>().SetText(price.ToString());
    }

    void Unlock()
    {
        unlocked = true;
        skinHandler.unlockedSkins[skinID] = true;
        skinDisplay.sprite = skinSprite;
        priceDisplay.transform.parent.gameObject.SetActive(false);
        AchievementManager.GetAchievement("NEW_SKIN");
    }
    void Purchase()
    {
        if(coinHandler.coinCount >= price)
        {
            coinHandler.coinCount -= price;
            activationSound = purchaseSound;
            Unlock();
        }
        else
        {
            activationSound = cantUnlockSound;
        }
    }
    public override void Activate()
    {
        if(unlocked)
        {
            activationSound = equipSound;
            skinHandler.UpdateSkin(skinID);
        }
        else
        {
            Purchase();
        }
    }
}
