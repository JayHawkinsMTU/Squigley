using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantParry : MessageEvent
{
    public GameObject effect;
    public override void Event(GameObject p)
    {
        Debug.Log("Granted parry");
        foreach(Parry parry in FindObjectsOfType<Parry>(true))
        {
            Parry.unlocked = true;
            parry.gameObject.SetActive(true);
            if(effect != null)
            {
                Instantiate(effect, parry.transform, false);
            }
        }
    }
}
