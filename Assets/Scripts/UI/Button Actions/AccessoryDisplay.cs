using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccessoryDisplay : Button 
{
    private int index = 0;
    [SerializeField] UICoinHandler coinHandler;
    [SerializeField] SkinMatrix matrix;
    [SerializeField] Image skinDisplay;
    float scale;
    [SerializeField] Image accessoryDisplay;
    [SerializeField] TMP_Text nameDisplay;
    [SerializeField] TMP_Text priceDisplay;
    [SerializeField] AudioClip cantBuy;
    [SerializeField] AudioClip buy;
    [SerializeField] AudioClip equip;
    GameObject player;
    AccessoryHandler handler;
    Accessory accessory;
    public override void Hover()
    {
        player = matrix.player;
        handler = player.transform.Find("Accessory").GetComponent<AccessoryHandler>(); //Assigns handler using player's children.
        UpdateDisplay();
    }
    public override void Activate()
    {
        if(AccessoryHandler.unlockedAccessories[index])
        {
            Equip();
        }
        else if(accessory != null)
        {
            PurchaseAcc();
        }
    }
    public void NextAcc()
    {
        index = Mathf.Clamp(index + 1, 0, handler.accessories.Length - 1);
        accessory = handler.accessories[index];
    }
    public void PrevAcc()
    {
        index = Mathf.Clamp(index - 1, 0, handler.accessories.Length - 1);
        accessory = handler.accessories[index];
    }
    private void PurchaseAcc()
    {
        if(coinHandler.coinCount >= accessory.price)
        {
            activationSound = buy;
            coinHandler.coinCount -= accessory.price;
            AccessoryHandler.unlockedAccessories[index] = true; //Marks accessory as unlocked.
            UpdateDisplay();
        }
        else
        {
            activationSound = cantBuy;
        }
    }
    private void UpdateDisplay()
    {
        if(accessory != null)
        {
            accessoryDisplay.enabled = true;
            accessoryDisplay.sprite = accessory.sprite;
            accessoryDisplay.color = accessory.color;
            accessoryDisplay.transform.localPosition = accessory.offset * scale;
            accessoryDisplay.transform.localScale = accessory.scale * accessory.sprite.bounds.size.x;
            nameDisplay.text = accessory.accessoryName;
            if(AccessoryHandler.unlockedAccessories[index])
            {
                priceDisplay.text = "Unlocked";
            }
            else
            {
                priceDisplay.text = accessory.price.ToString();
            }
        }
        else //At index 0 -- no accessory.
        {
            nameDisplay.text = "Nothing";
            accessoryDisplay.sprite = null;
            accessoryDisplay.enabled = false; //Without this line it gives squigley an afro.
            priceDisplay.text = "";
            skinDisplay.color = player.GetComponent<SpriteRenderer>().color;
        }
        
    }
    private void Equip()
    {
        activationSound = equip;
        handler.LoadAccessory(index);
        priceDisplay.text = "Equipped";
    }
    void Awake()
    {
        scale = skinDisplay.rectTransform.sizeDelta.x;
        buy = activationSound;
    }
}