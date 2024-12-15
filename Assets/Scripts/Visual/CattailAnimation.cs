using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattailAnimation : MonoBehaviour
{
    private const float LENGTH = 2 * Mathf.PI;
    private const float SPEED = 4;
    private const float DECAY = 1f;
    private const float AMPLITUDE = 8;
    private float timeCount = 20;
    [SerializeField] GameObject root;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            timeCount = 0;
            GetComponent<ParticleSystem>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeCount < LENGTH)
        {
            timeCount += Time.deltaTime;
            root.transform.eulerAngles = new Vector3(0,0, AMPLITUDE / Mathf.Clamp((DECAY * timeCount), 1, 25) * Mathf.Sin(SPEED * timeCount));
        }
    }
}
