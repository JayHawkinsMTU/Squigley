using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : Button
{
    [SerializeField] string sceneName;
    [SerializeField] DataPersistenceManager dpm;
    public override void Activate()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if(dpm != null)
        {
            dpm.SaveGame();
        }
    }
}
