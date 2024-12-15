using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[System.Serializable]
public class VoidMap
{
    public string mapName = "NEW_MAP";
    public List<VoidObject> objects = new();
    public ObjectList list;
    //public string mapDescription;

    /*Settings
    public bool multiplayerAvailable;
    public float gravity;*/

    public void UpdateWrapper()
    {
        list = new ObjectList(objects);
    }
    [System.Serializable]
    public class ObjectList
    {
        public List<VoidObject> objectList;
        public ObjectList(List<VoidObject> l)
        {
            objectList = l;
        }
    }
    
}
