using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICoinHandler : MonoBehaviour, IDataPersistence
{
    // No reason for this static variable to be on this script in particular. Call me lazy but I only put it here because it already has IDataPersistence.
    public static bool admin = false;
    public int coinCount = 0;
    public TMP_Text ui;
    public Movement player;
    public TMP_Text uiCheckpoint;
    public int checkPointLives;

    //Other Stats. These values should go up, but not down.
    public int totalCoinsCollected = 0;
    public int vasesBroken = 0;
    public int totalDeaths = 0;
    public int messagesTriggered = 0;
    public int blocksBroken = 0;
    [SerializeField] bool saveAndLoad = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Movement>();
    }
    

    // Update is called once per frame
    void Update()
    {
        checkPointLives = player.checkPointLives;
        if(ui != null)
        {
            ui.SetText(Mathf.Clamp(coinCount, 0, 999).ToString());
            if(checkPointLives > 0)
            {
                uiCheckpoint.SetText(checkPointLives.ToString());
            }
            else
            {
                uiCheckpoint.SetText("");
            }
        }
    }
    
    public void LoadData(SaveData data)
    {
        if(!saveAndLoad) return;
        this.coinCount = data.coinCount;
        this.checkPointLives = data.checkPointLives;
        this.totalCoinsCollected = data.totalCoinsCollected;
        this.vasesBroken = data.vasesBroken;
        this.totalDeaths = data.totalDeaths;
        this.messagesTriggered = data.messagesTriggered;
        this.blocksBroken = data.blocksBroken;
        admin = data.admin;
    }

    public void SaveData(ref SaveData data)
    {
        if(!saveAndLoad) return;
        data.coinCount = this.coinCount;
        data.checkPointLives = this.checkPointLives;
        data.totalCoinsCollected = this.totalCoinsCollected;
        data.vasesBroken = this.vasesBroken;
        data.totalDeaths = this.totalDeaths;
        data.messagesTriggered = this.messagesTriggered;
        data.blocksBroken = this.blocksBroken;
        data.admin = admin;
    }
}
