using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblivionGenerator : MonoBehaviour
{
    private const int ZONE_SIZE = 100;
    private const int MAX_X_DEVIATION = 13;
    private const int MAX_Y_DEVIATION = 1;
    private const int MIN_OBJECTS = 52;
    private const int MAX_OBJECTS = 58;
    private const int MIN_MISC_OBJECTS = 5;
    private const int MAX_MISC_OBJECTS = 20;
    private const float MIN_DIST_BETWEEN_OBJ = 2.5f;
    private int[,] STAGE_RANGES;
    public OblivionStage[] library; //Index 0 should be misc objects;
    public ScoreHandler scoreHandler;
    private int level;
    private int cursorY = -50;
    private bool generated = false;
    private bool unloading = false;
    private Wait unloadWait = new Wait(1);
    void Generate()
    {
        generated = true;
        GameObject nextZone = Instantiate(this.gameObject, new Vector2(0, transform.position.y + ZONE_SIZE), Quaternion.identity);
        scoreHandler.AddToZones(nextZone);
        level = scoreHandler.level;
        int objectCount = Random.Range(MIN_OBJECTS, MAX_OBJECTS);
        int miscObjectCount = Random.Range(MIN_MISC_OBJECTS, MAX_MISC_OBJECTS);
        int objX = 0;
        int objY = 0;
        int prevObjX = 0;
        int prevObjY = 0;
        //Load Platforms
        while(cursorY <= ZONE_SIZE / 2)
        {
            int objectLevel = Random.Range(STAGE_RANGES[level,0], STAGE_RANGES[level,1]);
            int objectIndex = Random.Range(0, library[objectLevel].prefabs.Length - 1);
            objX = Random.Range(-MAX_X_DEVIATION, MAX_X_DEVIATION);
            objY = cursorY + Random.Range(-MAX_Y_DEVIATION, MAX_Y_DEVIATION);
            if(Vector2.Distance(new Vector2(objX, objY), new Vector2(prevObjX, prevObjY)) < MIN_DIST_BETWEEN_OBJ)
            {
                continue; //Repositions current object if it's too close to the previous.
            }
            GameObject currentObj = library[objectLevel].prefabs[objectIndex];
            currentObj = Instantiate(currentObj, this.transform);
            currentObj.transform.localPosition = new Vector3(objX, objY, 1);

            cursorY += ZONE_SIZE / objectCount;
            prevObjX = objX;
            prevObjY = objY;
        }
        //Load coins
        for(int i = 0; i < miscObjectCount; i++)
        {
            int objectLevel = 0;
            int objectIndex = Random.Range(0, library[objectLevel].prefabs.Length);
            objX = Random.Range(-15, 15);
            objY = Random.Range(-50, 50);
            GameObject currentObj = library[objectLevel].prefabs[objectIndex];
            Instantiate(currentObj, this.transform);
            currentObj.transform.localPosition = new Vector3(objX, objY, 1);
        }
    }
    void Awake()
    {
        STAGE_RANGES = new int[9,2] {{1,1},{1,2},{1,3},{1,4},{1,5},{2,5},{3,5},{4,5},{5,5}};
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !generated) //Generates when any player crosses border.
        {
            Generate();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Movement>().playerID == 1) //Destroys only when player 1 exits.
        {
            if(collision.transform.position.y > transform.position.y) //Should only destroy topside in order to avoid massive gaps.
            {
                collision.transform.parent = null; //SHOULD prevent player from being destroyed when they are on a moving platform. Doesn't always.
                //Destroy(this.gameObject);
                unloading = true; //New solution: delay
            }
        }
    }
    void Update()
    {
        if(unloading)
        {
            unloadWait.Iterate();
            if(unloadWait.Complete())
            {
                Destroy(this.gameObject);
            }
        }
    }
}
