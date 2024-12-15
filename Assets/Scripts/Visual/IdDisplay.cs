using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdDisplay : MonoBehaviour, IDataPersistence
{
    public void LoadData(SaveData data)
    {
        this.gameObject.SetActive(data.hasId);
    }

    public void SaveData(ref SaveData data)
    {
        return;
    }

    
}
