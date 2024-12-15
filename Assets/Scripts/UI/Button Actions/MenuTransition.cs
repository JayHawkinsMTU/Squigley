using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransition : Button
{
    [SerializeField] GameObject menuOne;
    [SerializeField] GameObject menuTwo;
    [SerializeField] AudioSource audioSource; //Needs to be outside of scope of both menus in order to avoid being disabled.
    [SerializeField] DataPersistenceManager dataPersistenceManager;

    public override void Activate()
    {
        audioSource.PlayOneShot(activationSound, 0.5f);
        dataPersistenceManager.SaveGame();
        if(menuOne.activeInHierarchy)
        {
            menuOne.SetActive(false);
            menuTwo.SetActive(true);
        }
        else
        {
            menuOne.SetActive(true);
            menuTwo.SetActive(false);
        }
    }
    
}
