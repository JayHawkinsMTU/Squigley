using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EyeSpawner : MonoBehaviour
{
    public EyeStates eyeStates;
    public float minWait = 0.5f;
    public float maxWait = 4f;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public GameObject entity;
    public int count = 1;
    AudioSource audioSource;
    public AudioClip spawnSound;
    public AudioClip hurtSound;
    private bool despawning;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnEnable()
    {
        transform.position = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), transform.position.z);
        // This statement prevents the eye from being in the center platform
        if(Mathf.Abs(transform.position.y) < 1)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sign(transform.position.y), transform.position.z);
        }
        despawning = false;
        StopAllCoroutines();
        StartCoroutine(Phases());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !despawning)
        {
            eyeStates.StartBlink();
            eyeStates.Contract(true);
            audioSource.PlayOneShot(hurtSound, 0.6f);
            GetComponent<DespawnOnTimer>().StartDespawn();
            despawning = true;
        }
    }

    IEnumerator Phases()
    {
        eyeStates.StartUnblink();
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        for(int i = 0; i < count; i++)
        {
            eyeStates.Dilate(true);
            yield return new WaitForSeconds(0.5f);
            if(despawning) break;
            audioSource.PlayOneShot(spawnSound, 0.6f);
            Instantiate(entity, transform.position + new Vector3(0, 0, -4), Quaternion.identity, transform.parent);
            eyeStates.Idle();
            yield return new WaitForSeconds(0.5f);
        }
        eyeStates.StartBlink();
        GetComponent<DespawnOnTimer>().StartDespawn();
    }
}
