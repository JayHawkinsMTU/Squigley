using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryHandler : MonoBehaviour, IDataPersistence
{
    SpriteRenderer spi;
    SpriteRenderer playerSpi;
    private int curAcc = 0;
    public Accessory[] accessories;
    [SerializeField] Movement movement;
    public static bool[] unlockedAccessories = new bool[15]; //Static because its shared beween players.
    private int index = 0;
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        playerSpi = transform.parent.GetComponent<SpriteRenderer>();
        enabled = false;
        LoadAccessory(index);
    }
    public void LoadAccessory(int index)
    {
        Accessory newAcc = accessories[index];
        unlockedAccessories[index] = true; //Marks accessory index as unlocked.
        curAcc = index;
        if(newAcc != null)
        {
            if(newAcc.mathPlayerOrientation)
            {
                enabled = true;
            }
            else
            {
                enabled = false;
            }
            spi.sprite = newAcc.sprite;
            if(newAcc.matchPlayerColor)
            {
                spi.color = playerSpi.color;
            }
            else
            {
                spi.color = newAcc.color;
            }
            transform.localPosition = newAcc.offset;
            transform.localScale = newAcc.scale;
            spi.enabled = true;
        }
        else
        {
            spi.enabled = false; //Disables accessory render when slot is empty
            enabled = false;
        }
    }
    void Update()
    {
        spi.flipX = playerSpi.flipX;
    }
    public void LoadData(SaveData data)
    {
        index = data.accessories[movement.playerID - 1];
        unlockedAccessories = data.unlockedAccessories;
        Start();
    }
    public void SaveData(ref SaveData data)
    {
        int index = movement.playerID - 1;
        data.accessories[index] = curAcc;
        data.unlockedAccessories = unlockedAccessories;
    }
}
