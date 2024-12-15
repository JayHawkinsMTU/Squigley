using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    // Sin properties
    [SerializeField] float frequency, magnitudeScale, magnitudeShift;
    float initposY;
    float initScale;
    [SerializeField] float rotateSpeed;
    public GameObject playerCamera;
    public int orientation;
    AudioSource audioSource;
    public AudioClip sound;
    public bool giveFastFallCharge = true;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        Movement p = other.GetComponent<Movement>();
        if(p != null)
        {
            p.FlipGrav(orientation);
            StartCoroutine(FlipCam());
            audioSource.PlayOneShot(sound, 0.8f);
            if(giveFastFallCharge) p.fastfallCharge = true;
        }
    }
    IEnumerator FlipCam()
    {
        int target;
        if(orientation == -1) target = 180;
        else target = 0;
        while(playerCamera.transform.eulerAngles.z != target)
        {
            playerCamera.transform.eulerAngles = Vector3.MoveTowards(playerCamera.transform.eulerAngles, new Vector3(0, 0, target), Time.deltaTime * rotateSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnEnable()
    {
        playerCamera.transform.eulerAngles = Vector3.zero;
    }
    void Start()
    {
        initposY = transform.position.y;
        initScale = transform.localScale.y;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Animation handling
        transform.localScale = new Vector3(transform.localScale.x, initScale + Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitudeScale , 0);
        transform.position = new Vector3(transform.position.x, initposY + Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitudeShift, 0);
    }
}
