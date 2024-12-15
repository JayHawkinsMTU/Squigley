using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public DataPersistenceManager dpm;
    VoidMap currentMap = new();
    GameObject[] prefabs;
    public GameObject addedObjects;
    [SerializeField] GameObject emptyAddedObjects;
    void Awake()
    {
        prefabs = GetComponent<DebugTerminal>().prefabs;
    }
    public void Save()
    {
        SaveAs(currentMap.mapName);
    }
    public void SaveAs(string name)
    {
        currentMap.objects = new(); //Resets objets in map to be empty sense data exists in scene.
        foreach(Transform t in addedObjects.transform) //Loops through children of addedObjects
        {
            //Create VoidObject so serialization works.
            VoidObject vo = new();
            vo.index = int.Parse(t.gameObject.name.Split(' ')[0]); //Possibly the dumbest way possible of assigning this. Gets the index from gameobject name.
            vo.posX = t.localPosition.x;
            vo.posY = t.localPosition.y;
            vo.sizeX = t.localScale.x;
            vo.sizeY = t.localScale.y;
            vo.rotation = t.eulerAngles.z;
            currentMap.objects.Add(vo);
            Debug.Log("Added object: " + vo.index);
        }
        currentMap.mapName = name;
        currentMap.list = new VoidMap.ObjectList(currentMap.objects);
        dpm.SaveVoidMap(currentMap, currentMap.mapName);
    }
    public void Load(string loadName)
    {
        Destroy(this.addedObjects);
        currentMap = dpm.LoadVoidMap(loadName);
        addedObjects = Instantiate(emptyAddedObjects) as GameObject;
        foreach(VoidObject obj in currentMap.objects)
        {
            if(obj == null) continue;
            GameObject o = Instantiate(prefabs[obj.index], new Vector2(obj.posX, obj.posY), Quaternion.identity, addedObjects.transform) as GameObject;
            o.transform.localScale = new Vector2(obj.sizeX, obj.sizeY);
            o.transform.eulerAngles = new Vector3(0, 0, obj.rotation);
        }
    }
}
