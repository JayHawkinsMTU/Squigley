using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour, IDataPersistence
{
    public GameObject eventSystem;
    bool found = false;
    string[] spawnMessage;
    [SerializeField] string[] triggeredMessage;
    [SerializeField] int id = 0; //0 Should correspond to unassigned ID's, there should be none at the end of development.
    public bool oneTimeOnly = true;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player") && (!oneTimeOnly || !found))
        {
            eventSystem.GetComponent<UICoinHandler>().messagesTriggered++;
            UtilityText.primaryInstance.DisplayMsg("New Message!", Color.green);
            Message.newMessage = true;
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
        if(data.unlockedMessages[id])
        {
            found = true;
            if(oneTimeOnly) {
                this.enabled = false;
            }
        }
    }
    public void SaveData(ref SaveData data)
    {
        data.unlockedMessages[id] = this.found;
    }
}
