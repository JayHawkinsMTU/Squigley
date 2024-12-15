using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadSceneOnVideoEnd : MonoBehaviour
{
    [SerializeField] VideoPlayer video;
    [SerializeField] string sceneName;
    [SerializeField] bool skippable = true;
    void Awake()
    {
        video.loopPointReached += LoadMenu;
        Cursor.visible = false;
    }
    void LoadMenu(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneName);
        Cursor.visible = true;
    }

    void Update()
    {
        if(skippable && GameInput.Interact(1))
        {
            LoadMenu(video);
        }
    }
}
