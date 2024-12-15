using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpin : MonoBehaviour
{
    [SerializeField] float degPerSecond = 30;
    [SerializeField] float magnitude = 0.5f;
    [SerializeField] float frequency = 4;
    SpriteRenderer spi;
    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject toEnable;
    private float initY;
    private float time = 0;
    void Awake()
    {
        spi = GetComponent<SpriteRenderer>();
        initY = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.eulerAngles.y + degPerSecond * Time.deltaTime, transform.rotation.z);
        if(transform.eulerAngles.y > 90 && transform.eulerAngles.y < 270)
        {
            spi.sprite = back;
        }
        else
        {
            spi.sprite = front;
        }
        time += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, initY + magnitude * Mathf.Sin(time * frequency), transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            if(audioSource != null) audioSource.Play();
            if(toEnable != null) toEnable.SetActive(true);
        }
    }
}
