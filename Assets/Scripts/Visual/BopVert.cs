using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BopVert : MonoBehaviour
{
    [SerializeField] private float magnitude = -0.125f; //Moves 1 pixel down by default.
    [SerializeField] private float frequency = 1f; //1 time per second by default
    private bool direction = false;
    IEnumerator Bob()
    {
        while(true)
        {
            if(direction)
            {
                transform.Translate(0, magnitude, 0);
            }
            else
            {
                transform.Translate(0, -magnitude, 0);
            }
            direction = !direction;
            yield return new WaitForSeconds(1 / frequency);
        }
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Bob());
    }
    void Start()
    {
        OnEnable();
    }
}
