using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] bool spawnOnStart;
    [SerializeField] bool randomTime;
    [SerializeField] bool keepOnDisable;
    [SerializeField] float minBoundTime = 1; //Minimum amount of time to spawn if random
    [SerializeField] float maxBoundTime = 10; //Maximum amount of time to spawn if random
    [SerializeField] float respawnTime = 5; //Amount to wait until spawn otherwise
    [SerializeField] GameObject prefab;
    private GameObject instance;
    private float time;
    private bool respawning; //Marks when the coroutine is running
    private SpriteRenderer spi; //For ease of editing
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        spi.enabled = false;
        if(spawnOnStart && instance == null)
        {
            Spawn();
        }
        NewTime();
        
    }
    void OnDisable()
    {
        StopAllCoroutines();
        respawning = false;
        if(!keepOnDisable) Destroy(instance);
    }
    private void NewTime()
    {
        if(randomTime)
        {
            time = Random.Range(minBoundTime, maxBoundTime);
        }
        else
        {
            time = respawnTime;
        }
    }
    void Spawn()
    {
        instance = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
        NewTime(); //If random, a new time is set each spawn
    }
    IEnumerator Respawn()
    {
        respawning = true;
        yield return new WaitForSeconds(time);
        Spawn();
        respawning = false;
    }
    void Update()
    {
        if(!respawning && instance == null)
        {
            StartCoroutine(Respawn());
        }
    }
}
