using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour, IDataPersistence
{
    public int id = 0;
    [SerializeField] bool bossDoor = false;
    public GameObject door;
    public SpriteRenderer sprite;
    public bool unlocked = false;
    public void Lock()
    {
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,1f);
        door.GetComponent<SpriteRenderer>().enabled = true;
        door.GetComponent<BoxCollider2D>().enabled = true;
        unlocked = false;
    }
    public void Unlock()
    {
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,0.03f);
        door.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<BoxCollider2D>().enabled = false;
        unlocked = true;
    }
    void Start()
    {
        if(bossDoor && unlocked) Unlock();
    }
    public void LoadData(SaveData data)
    {
        if(!bossDoor && data.unlockedDoors[id])
        {
            Unlock();
        }
    }
    public void SaveData(ref SaveData data)
    {
        if(!bossDoor) data.unlockedDoors[id] = unlocked;
    }
}
