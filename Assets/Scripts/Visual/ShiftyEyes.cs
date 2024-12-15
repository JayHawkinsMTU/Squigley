using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftyEyes : MonoBehaviour
{
    SpriteRenderer spi;
    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        StartCoroutine(Animation());
    }

    void OnEnable()
    {
        Start();
    }

    IEnumerator Animation()
    {
        while(enabled)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));
            spi.flipX = !spi.flipX;
        }
    }
}
