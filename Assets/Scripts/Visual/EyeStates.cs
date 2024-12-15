using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeStates : MonoBehaviour
{
    [SerializeField] Sprite[] blink;
    [SerializeField] Sprite[] pupils; // 0 - contracted, 1 - normal, 2 - dilated 
    [SerializeField] float blinkWaitMin = 0.5f;
    [SerializeField] float blinkWaitMax = 6.65f;
    [SerializeField] float blinkFPS = 6f;
    [SerializeField] SpriteRenderer eyelid;
    [SerializeField] GameObject iris;
    [SerializeField] SpriteRenderer pupil;
    public void Idle()
    {
        StopAllCoroutines();
        StartCoroutine(IdleAnimation());
        pupil.sprite = pupils[1];

        IEnumerator IdleAnimation()
        {
            while(true)
            {
                yield return new WaitForSeconds(Random.Range(blinkWaitMin, blinkWaitMax));
                StartCoroutine(Unblink());
            }
        } 
    }

    public void StartUnblink()
    {
        StopAllCoroutines();
        StartCoroutine(Unblink());
    }

    IEnumerator Unblink()
    {
        for(int i = blink.Length - 1; i >= 0; i--)
        {
            eyelid.sprite = blink[i];
            yield return new WaitForSeconds(1f / blinkFPS);
        }
    }
    public void StartBlink()
    {
        StopAllCoroutines();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        for(int i = 0; i < blink.Length; i++)
        {
            eyelid.sprite = blink[i];
            yield return new WaitForSeconds(1f / blinkFPS);
        }
    }

    public void Contract(bool vibrate = false)
    {
        StopAllCoroutines();
        eyelid.sprite = blink[0]; // Opens wide
        pupil.sprite = pupils[0];
        if(vibrate) StartCoroutine(VibrateIris());
    }

    public void Dilate(bool vibrate = false)
    {
        StopAllCoroutines();
        eyelid.sprite = blink[0]; // Opens wide
        pupil.sprite = pupils[2];
        if(vibrate) StartCoroutine(VibrateIris());
    }

    public void SetFollow(bool enabled)
    {
        Vector3 irisPos = iris.transform.position;
        EyeFollow eye = GetComponent<EyeFollow>();
        if(eye != null) eye.enabled = enabled;
        iris.transform.position = irisPos;
    }

    IEnumerator VibrateIris()
    {
        Vector3 init = iris.transform.position;
        SetFollow(false);
        while(true)
        {
            iris.transform.position = new Vector3(init.x + Random.Range(-.1f, .1f), init.y + Random.Range(-.1f, .1f), init.z);
            yield return new WaitForEndOfFrame();
        }
    }

    void Start()
    {
        Idle();
    }

    void OnDisable()
    {
        iris.transform.localPosition = new Vector3(0, 0, iris.transform.localPosition.z);
    }

    void OnEnable()
    {
        SetFollow(true);
        Idle();
    }

}
