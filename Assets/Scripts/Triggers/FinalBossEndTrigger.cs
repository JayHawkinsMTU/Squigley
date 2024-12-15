using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossEndTrigger : MonoBehaviour
{
    public VisualEffect whiteout;
    public float waitBeforeLoading;
    public string sceneName;
    private bool running = false;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!running && collider.CompareTag("Player"))
        {
            running = true;
            StartCoroutine(End());
        }
    }
    IEnumerator End()
    {
        Debug.Log("Starting ending");
        whiteout.StartEffect(4, VisualEffect.FADEIN);
        yield return new WaitForSeconds(waitBeforeLoading);
        SceneManager.LoadScene(sceneName);
    }
}
