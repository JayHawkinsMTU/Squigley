using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenePortal : MonoBehaviour, IDataPersistence
{
    public AudioSource audioSource;
    [SerializeField] AudioClip warp;
    [SerializeField] string sceneName;
    [SerializeField] bool transferObjects;
    [SerializeField] Vector3 nextScenePosition;
    [SerializeField] bool saveOnEnter = true;
    DataPersistenceManager dataPersistenceManager;
    TimeData timeData;
    private bool transitioning = false;
    Wait wait;

    void Start()
    {
        transitioning = false;
        audioSource = GetComponent<AudioSource>();
        if(saveOnEnter) dataPersistenceManager = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        timeData = GameObject.Find("EventSystem").GetComponent<TimeData>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            transitioning = true;
            if(transferObjects)
            {
                DontDestroyOnLoad(collision);
                DontDestroyOnLoad(GameObject.Find("Persisting Objects"));
            }
            audioSource.PlayOneShot(warp, 0.5f);
            timeData.EndTime();
            if(saveOnEnter) dataPersistenceManager.SaveGame();
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            collision.gameObject.transform.position = nextScenePosition;
        }
    }

    public void SaveData(ref SaveData data)
    {
        if(transitioning) data.sceneName = this.sceneName;
    }
    public void LoadData(SaveData data)
    {
        return;
    }
}
