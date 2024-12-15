using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSlash : MonoBehaviour
{
    const float GRACE = 0.155f;
    public float speed = 5f; // Speed at which crosshair follows player
    public float trackTime = 1.5f;
    private Vector3 initPos;
    private AudioSource audioSource;
    public AudioClip trackingSound;
    public AudioClip slashingSound;
    private bool tracking = true;
    private GameObject player;
    public GameObject slash;
    private SpriteRenderer slashSprite;
    public GameObject crosshair;

    void Awake()
    {
        initPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        slashSprite = slash.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Reset();
        StartCoroutine(Phases());
    }

    void Reset()
    {
        slash.SetActive(false);
        slashSprite.color = new Color(slashSprite.color.r, slashSprite.color.g, slashSprite.color.b, 1);
        crosshair.SetActive(true);
        tracking = true;
        transform.position = initPos;
        slash.GetComponent<PolygonCollider2D>().enabled = true;
    }

    IEnumerator Phases()
    {
        // Track Player
        audioSource.PlayOneShot(trackingSound, 0.6f);
        tracking = true;
        yield return new WaitForSeconds(trackTime);
        
        // Grace / Warning
        tracking = false;
        yield return new WaitForSeconds(GRACE);

        // Slash
        audioSource.PlayOneShot(slashingSound, 0.75f);
        slash.SetActive(true);
        crosshair.SetActive(false);
        slash.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        yield return new WaitForSeconds(GRACE);
        slash.GetComponent<PolygonCollider2D>().enabled = false;
        while(slashSprite.color.a > .05f)
        {
            slashSprite.color = new Color(slashSprite.color.r, slashSprite.color.g, slashSprite.color.b, slashSprite.color.a - Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        slashSprite.color = new Color(slashSprite.color.r, slashSprite.color.g, slashSprite.color.b, 0);


        yield return new WaitForSeconds(.75f);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(tracking)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Mathf.Clamp(Vector2.Distance(transform.position, player.transform.position), .65f, 50) * Time.deltaTime * speed);
        }
    }
}
