using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nulroy : MonoBehaviour
{
    public AudioListener listener;
    public AudioLowPassFilter filter;
    public VisualEffect visualEffect;
    public GameObject player1;
    public float speed = 2;
    public float maxDist = 100;
    public float catchupDist = 80;
    void Start()
    {
        OnEnable();
    }
    void OnEnable()
    {
        filter.enabled = true;
        visualEffect.StartEffect(20, VisualEffect.FADEOUT);
    }

    void OnDisable()
    {
        filter.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(transform.position, player1.transform.position);
        if(dist < maxDist)
        {
            transform.position = Vector2.MoveTowards(transform.position, player1.transform.position, speed * Time.deltaTime);
            filter.cutoffFrequency = Mathf.Clamp(22000 * (dist / (maxDist / 2)), 10, 22000);
        }
        else
        {
            // When distance exceeds maxDist, it teleports somewhere exactly catchupDist away.
            float rad = Random.Range(0, 6.28f);
            transform.position = new Vector2(player1.transform.position.x + catchupDist * Mathf.Cos(rad), player1.transform.position.y + catchupDist * Mathf.Sin(rad));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Application.Quit();
        }
    }
}
