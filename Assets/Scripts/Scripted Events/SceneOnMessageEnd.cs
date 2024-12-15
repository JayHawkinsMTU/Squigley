using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOnMessageEnd : MonoBehaviour
{
    [SerializeField] NewMessageHandler nmh;
    [SerializeField] string sceneName;
    // Update is called once per frame
    void Update()
    {
        if(nmh.active && nmh.msg.isMessageDone())
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
    }
}
