using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EraseDataButton : Button
{
    [SerializeField] DataPersistenceManager dataPersistenceManager;
    [SerializeField] MenuTransition mtrs;
    [SerializeField] AudioSource outerAudio;

    public override void Activate()
    {
        dataPersistenceManager.NewGame();
        dataPersistenceManager.LoadGame();
        if(mtrs != null) 
        {
            mtrs.Activate();
            outerAudio.PlayOneShot(activationSound, 0.85f);
        }
    }

    public override void Hover()
    {
        if(GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().color = Color.red;
        }
    }
    public override void Unhover()
    {
        if(GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().color = Color.white;
        }
    }
}
