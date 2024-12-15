using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameDecision : Decision
{
    const float FADE_TIME = 3f;
    const float SCENE_LOAD_PADDING = 1f;
    [SerializeField] VisualEffect fadeToBlack;
    [SerializeField] string terminalSceneName, overworldSceneName, finalBossSceneName;
    [SerializeField] TimeData timeData;
    [SerializeField] GameObject theosMsg;
    AudioSource audioSource;
    [SerializeField] AudioClip endGameSound;
    private int option = 0;
    private string sceneToLoad = "No Scene Assigned";
    Wait loadWait = new Wait(FADE_TIME + SCENE_LOAD_PADDING);
    [SerializeField] TheosAnimation theosAnimation;
    [SerializeField] MoveTo moveTo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Choose(int option)
    {
        decisionMatrix.Close();
        switch(option)
        {
            case 1: //End Game
                this.option = option;
                sceneToLoad = terminalSceneName;
                timeData.EndTime();
                fadeToBlack.StartEffect(FADE_TIME, VisualEffect.FADEIN);
                audioSource.PlayOneShot(endGameSound, 0.7f);
                break;
            case 2: //Continue Game
                this.option = option;
                sceneToLoad = overworldSceneName;
                fadeToBlack.StartEffect(FADE_TIME, VisualEffect.FADEIN);
                break;
            case 3: //Final Boss and true ending
                StartCoroutine(StartFinalBoss());
                break;
        }
    }
    void Update()
    {
        if(option == 1 || option == 2)
        {
            loadWait.Iterate();
            if(loadWait.Complete())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    IEnumerator StartFinalBoss()
    {
        theosAnimation.enabled = false;
        moveTo.enabled = true;
        theosMsg.SetActive(false); //Ensures that the player cannot talk to TheOs while he moves.
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(finalBossSceneName);
    }
}
