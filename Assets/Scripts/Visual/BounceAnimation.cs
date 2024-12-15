using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    private bool animationRunning = false;
    public GameObject block;
    float timeCount = 0;
    float animationLength = .8f;
    string direction = "north";
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip boing;
    // Start is called before the first frame update
    void Bounce(string direction)
    {
        timeCount += Time.deltaTime;

        if(direction == "north")
        {
            block.transform.localScale = new Vector3(block.transform.localScale.x, .75f + .25f * Mathf.Cos(Mathf.PI * 5 * timeCount));
            block.transform.localPosition = new Vector3(block.transform.localPosition.x, (-.125f + Mathf.Cos(Mathf.PI * 5 * timeCount) / 8));
        }
        else if(direction == "west")
        {
            block.transform.localScale = new Vector3(.75f + .25f * Mathf.Cos(Mathf.PI * 5 * timeCount), block.transform.localScale.y);
            block.transform.localPosition = new Vector3(.125f - Mathf.Cos(Mathf.PI * 5 * timeCount) / 8, block.transform.localPosition.y);
        }
        else if(direction == "east")
        {
            block.transform.localScale = new Vector3(.75f + .25f * Mathf.Cos(Mathf.PI * 5 * timeCount), block.transform.localScale.y);
            block.transform.localPosition = new Vector3((-.125f + Mathf.Cos(Mathf.PI * 5 * timeCount) / 8), block.transform.localPosition.y);
        }
        else if(direction == "south")
        {
            block.transform.localScale = new Vector3(block.transform.localScale.x, .75f + .25f * Mathf.Cos(Mathf.PI * 5 * timeCount));
            block.transform.localPosition = new Vector3(block.transform.localPosition.x, (.125f - Mathf.Cos(Mathf.PI * 5 * timeCount) / 8));
        }

        //Stops animation when it has been going for longer than the length.
        if(timeCount >= animationLength)
        {
            Stop();
        }
    }
    //Starts animation. Publicly accessible.
    public void Play(string direction)
    {
        Stop();
        animationRunning = true;
        this.direction = direction;
        audioSource.PlayOneShot(boing, 0.5f);
    }
    //Stops animation and resets transfom values that may have been affected by the animation. Publicly accessible.
    public void Stop()
    {
        timeCount = 0;
        animationRunning = false;
        block.transform.localScale = new Vector3(1,1);
        block.transform.localPosition = new Vector3(0,0);
    }
    void Start()
    {
        //Play("north");
    }

    // Update is called once per frame
    void Update()
    {
        if(animationRunning)
        {
            Bounce(direction);
        }
    }
}
