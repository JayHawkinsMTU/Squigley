using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEvent : MessageEvent
{
    [SerializeField] string sceneName;
    public override void Event(GameObject p)
    {
        SceneManager.LoadScene(sceneName);
    }
}
