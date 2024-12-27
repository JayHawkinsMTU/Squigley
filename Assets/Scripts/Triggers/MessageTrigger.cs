using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour, IDataPersistence
{
    public GameObject eventSystem;
    bool found = false;
    string[] spawnMessage;
    [SerializeField] string[] triggeredMessage;
    // negative if you don't want to load or save data
    [SerializeField] int id = 0; 
    public bool oneTimeOnly = true;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player") && (!oneTimeOnly || !found))
        {
            eventSystem.GetComponent<UICoinHandler>().messagesTriggered++;
            if(id >= 0)
            {
                UtilityText.primaryInstance.DisplayMsg("New Message!", Color.green);
                Message.newMessage = true;
            }
            Message.globalMessage = triggeredMessage;
            found = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    public void LoadData(SaveData data)
    {
        if(id >= 0 && data.unlockedMessages[id])
        {
            found = true;
            if(oneTimeOnly) {
                this.enabled = false;
            }
        }
    }
    public void SaveData(ref SaveData data)
    {
        if(id < 0) return;
        data.unlockedMessages[id] = this.found;
    }
}
