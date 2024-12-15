using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeDamage : MonoBehaviour
{
    public Vector3 to;
    public GameObject platformRegion;
    public GameObject arena;
    public EyeStates eyeStates;
    List<Collider2D> disabled = new(); // List to ensure that both players return to normal collision after hit
    private bool hitting = false;
    public bool visual = false;
    AudioSource audioSource;
    public AudioClip hurtSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!visual)
            {
                other.enabled = false;
                disabled.Add(other);
            }
            audioSource.PlayOneShot(hurtSound, 0.9f);
            if(!hitting) StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        eyeStates.Contract(true);
        yield return new WaitForSeconds(1.5f);
        if(visual) yield break;
        AttackManager.primaryInstace.NextPhase();
        // Re-enables collision and sends back to start.
        foreach(Collider2D collider in disabled)
        {
            collider.enabled = true; 
            collider.GetComponent<WrapScreen>().enabled = true;
            collider.transform.position = to;
        } 
        arena.SetActive(true);
        platformRegion.SetActive(false);
    }
}
