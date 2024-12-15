using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpywareEnable : MonoBehaviour
{
    [SerializeField] GameObject psychic;
    [SerializeField] GameObject entryPortal;
    [SerializeField] VisualEffect vfx;
    [SerializeField] AudioSource audioSource;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Hazards"))
        {
            Destroy(psychic);
            entryPortal.SetActive(true);
            vfx.StartEffect(1, VisualEffect.FLASHBLINK);
            audioSource.Play();
            Destroy(this.gameObject);
        }
    }
}
