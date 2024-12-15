using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnTimer : MonoBehaviour
{
    public float lifespan = 10; // Lifespan of gameobject
    public SpriteRenderer[] spis; // All sprites to be faded out
    public bool destroy = false; // Disables if false, destroys if true
    public bool fadeIn = false;
    
    void OnEnable()
    {
        if(fadeIn) StartCoroutine(FadeIn());
        else SetOpacties(1);
        StartCoroutine(DespawnTimer());
    }

    IEnumerator FadeIn()
    {
        float opacity = 0;
        while(opacity < 1)
        {
            SetOpacties(opacity);
            opacity += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SetOpacties(1);
    }
    void SetOpacties(float o)
    {
        foreach(SpriteRenderer spi in spis)
        {
            spi.color = new Color(spi.color.r, spi.color.g, spi.color.b, o);
        }
    }

    void OnDisable()
    {
        Despawn();
    }

    public void Despawn()
    {
        if(destroy) Destroy(this.gameObject);
        else gameObject.SetActive(false);
    }

    public void StartDespawn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        float opacity = 1;
        while(opacity > 0)
        {
            SetOpacties(opacity);
            opacity -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Despawn();
    }
    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(lifespan);
        StartDespawn();
    }
}
