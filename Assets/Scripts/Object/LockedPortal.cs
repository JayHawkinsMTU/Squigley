using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedPortal : MonoBehaviour, IDataPersistence
{
    public int id = 0; //Uses same data as doors because why not yk.
    public GameObject linkedPortal; //Linked portal should have same script and ID.
    public bool unlocked = false;
    public void Lock()
    {
        this.gameObject.SetActive(false);
        linkedPortal.SetActive(false);
        unlocked = false;
        linkedPortal.GetComponent<LockedPortal>().unlocked = false;
    }
    public void Unlock()
    {
        this.gameObject.SetActive(true);
        linkedPortal.SetActive(true);
        unlocked = true;
        linkedPortal.GetComponent<LockedPortal>().unlocked = true;
    }
    public void LoadData(SaveData data)
    {
        if(data.unlockedDoors[id])
        {
            Unlock();
        }
        else // This statement isn't entirely necessary, but a good failsafe if I accidentally leave the gameobject enabled.
        {
            Lock();
        }
    }
    public void SaveData(ref SaveData data)
    {
        data.unlockedDoors[id] = this.unlocked;
    }
}
