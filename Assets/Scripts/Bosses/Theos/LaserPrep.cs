using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPrep : MonoBehaviour
{
    public float trackLength = 5; // Time of tracking phase
    public float laserLength = 3; // Time to wait before going back down
    public float warning = 1; // Time of warning
    public EyeStates eyeStates;
    public GameObject laser;
    private Vector3 initPos;
    private GameObject player;
    private bool tracking = true;
    private AudioSource audioSource;
    public AudioClip warningSound;
    private bool timeCondition = false;
    private float timeInRange = 0;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        initPos = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        tracking = true;
        timeCondition = false;
        timeInRange = 0;
        StartCoroutine(Phases());
    }
    void OnDisable()
    {
        transform.position = initPos;
    }
    IEnumerator TimeCondition()
    {
        yield return new WaitForSeconds(trackLength);
        timeCondition = true;
    }
    bool DistanceCondition()
    {
        if(Mathf.Abs(player.transform.position.y - transform.position.y) < 1.5f)
        {
            timeInRange += Time.deltaTime;
            if(timeInRange > 0.5f)
            {
                return true;
            }
        }
        else
        {
            timeInRange = 0;
        }
        return false;
    }
    IEnumerator Phases()
    {
        //Track
        StartCoroutine(TimeCondition());
        while(!timeCondition && !DistanceCondition())
        {
            yield return new WaitForEndOfFrame();
        }
        tracking = false;

        //Warn
        eyeStates.Contract();
        eyeStates.SetFollow(false);
        audioSource.PlayOneShot(warningSound, 0.85f);
        yield return new WaitForSeconds(warning);

        //Laser
        eyeStates.Contract(true);
        Instantiate(laser, new Vector3(0, transform.position.y, -2), Quaternion.identity);
        yield return new WaitForSeconds(laserLength);

        //Close
        eyeStates.Idle();
        eyeStates.SetFollow(true);
        while(Mathf.Abs(transform.position.y - initPos.y) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos, Vector2.Distance(transform.position, initPos) * Time.deltaTime * 3);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            Vector3 goal = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, goal, Mathf.Clamp(Vector2.Distance(transform.position, goal), 0.25f, 100) * Time.deltaTime);
        }
    }
}
