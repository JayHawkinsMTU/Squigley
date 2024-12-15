using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pause : MonoBehaviour
{
    [SerializeField] CameraUpgrades cameraUpgrades;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TMP_Text text; //"Paused"
    [SerializeField] TMP_Text text2; //"Interact to quit"
    [SerializeField] UtilityText text3; //"Saving..."
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    public bool paused  = false;
    private bool utilityPaused = false;
    private bool deadzoneReset;
    [SerializeField] RectTransform upArrow, downArrow;
    [SerializeField] bool quitToDesktop = false;
    private Vector2 upArrowStart;
    private Vector2 downArrowStart;
    [SerializeField] string menuName = "Main Menu";
    // Start is called before the first frame update
    void Start()
    {
        upArrowStart = upArrow.anchoredPosition;
        downArrowStart = downArrow.anchoredPosition;
    }

    public void UtilityPause()
    {
        paused = true;
        Time.timeScale = 0;
        utilityPaused = true;
    }
    public void UtilityUnpause()
    {
        paused = false;
        Time.timeScale = 1;
        utilityPaused = false;
    }

    public void ActivatePause()
    {
        if(cameraUpgrades != null)
            cameraUpgrades.UpdateDisplay();
        paused = true;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        audioSource.PlayOneShot(audioClip, 0.5f);
    }

    private void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!utilityPaused && GameInput.Pause()) //Pause
        {
            if(paused)
            {
                // Unpauses when paused.
                Unpause();
            }
            else
            {
                // Pauses when unpaused.
                ActivatePause();
            }
        }
        if(paused) //Pause Actions
        {
            if(!utilityPaused && GameInput.Interact(1))
            {
                text3.DisplayMsg("saving...", new Color(90,90,90));
                Time.timeScale = 1;
                if(DataPersistenceManager.instance != null)
                    DataPersistenceManager.instance.SaveGame();
                if(!quitToDesktop)
                {
                    SceneManager.LoadSceneAsync(menuName, LoadSceneMode.Single);
                }
                else
                {
                    Application.Quit();
                }
            }
            else if(!utilityPaused && GameInput.UIDown())
            {
                deadzoneReset = false;
                cameraUpgrades.ZoomIn();
            }
            else if(!utilityPaused && GameInput.UIUp())
            {
                deadzoneReset = false;
                cameraUpgrades.ZoomOut();
            }
            else if(!deadzoneReset && Input.GetAxis("Vertical") < .5f && Input.GetAxis("Vertical") > -.5f)
            {
                deadzoneReset = true;
            }
            if(upArrow != null && downArrow != null)
            {
                if(Input.GetKey("up"))
                {
                    upArrow.anchoredPosition = new Vector2(upArrowStart.x, upArrowStart.y + 3);
                }
                else
                {
                    upArrow.anchoredPosition = upArrowStart;
                }
                if(Input.GetKey("down"))
                {
                    downArrow.anchoredPosition = new Vector2(downArrowStart.x, downArrowStart.y - 3);
                }
                else
                {
                    downArrow.anchoredPosition = downArrowStart;
                }
            }
        }
    }
}
