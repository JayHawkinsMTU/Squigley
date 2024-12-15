using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectList
    {
        List<VoidObject> objects;
        public ObjectList(List<VoidObject> l)
        {
            objects = l;
        }
    }
