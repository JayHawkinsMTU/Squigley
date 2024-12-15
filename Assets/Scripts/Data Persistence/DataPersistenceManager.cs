using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public SaveData saveData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There are more than one data persistence managers in the scene.");
        }
        instance = this;
    }
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this.saveData = new SaveData();
        saveData.resolution = Screen.currentResolution;
        SaveGame();
    }
    public void SaveGame()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if(this.saveData == null)
        {
            // Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref saveData);
        }

        dataHandler.Save(saveData);
    }
    public void LoadGame()
    {
        this.saveData = dataHandler.Load();
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        if(this.saveData == null) {
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(saveData);
        }
    }
    public void SaveVoidMap(VoidMap data, string mapName)
    {
        dataHandler.SaveVoidMap(data, mapName);
    }
    public VoidMap LoadVoidMap(string mapName)
    {
        return dataHandler.LoadVoidMap(mapName);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
