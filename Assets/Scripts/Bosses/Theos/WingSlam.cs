using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSlam : MonoBehaviour
{
    public float force = 100;
    public GameObject[] wings;
    private float[] initZs;
    private Vector3 initPos;
    public Vector3 endPos;
    private AudioSource audioSource;
    public AudioClip wingSound;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        initZs = new float[wings.Length];
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < wings.Length; i++)
        {
            initZs[i] = wings[i].transform.eulerAngles.z;
        }
    }

    void OnEnable()
    {
        StartCoroutine(Phases());
    }

    void OnDisable()
    {
        SetRotation(0);
    }

    IEnumerator Phases()
    {
        float distance = Vector3.Distance(transform.position, endPos);
        while(distance > 1)
        {
            distance = Vector3.Distance(transform.position, endPos);
            transform.position = Vector3.MoveTowards(transform.position, endPos, distance * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        float modifier = 0;
        while(modifier < 30)
        {
            SetRotation(modifier);
            modifier += Time.deltaTime * (40 - modifier);
            yield return new WaitForEndOfFrame();
        }
        ApplyWind();
        while(modifier > -30)
        {
            SetRotation(modifier);
            modifier -= Time.deltaTime * 120;
            yield return new WaitForEndOfFrame();
        }
        while(Vector3.Distance(transform.position, initPos) > 1)
        {
            distance = Mathf.Clamp(Vector3.Distance(transform.position, endPos), 1, 10);
            transform.position = Vector3.MoveTowards(transform.position, initPos, distance * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    void SetRotation(float r)
    {
        int i = 0;
        foreach(GameObject wing in wings)
        {
            wing.transform.eulerAngles = new Vector3(wing.transform.eulerAngles.x, wing.transform.eulerAngles.y, initZs[i] + r);
            i++;
        }
    }

    void ApplyWind()
    {
        Rigidbody2D[] rbs = FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        audioSource.PlayOneShot(wingSound, 0.8f);
        foreach(Rigidbody2D rb in rbs)
        {
            rb.velocity -= new Vector2(0, force);
        }
    }
}
