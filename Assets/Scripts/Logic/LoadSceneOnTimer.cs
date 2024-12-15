using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTimer : MonoBehaviour
{
    public string sceneName;
    public float timeInSeconds;

    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}
