using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivate : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] monoBehaviours;
    public Vector3 initPosition;

    void Start()
    {
        initPosition = transform.position;
    }
    public void Activate()
    {
        for(int i = 0; i < monoBehaviours.Length; i++)
        {
            monoBehaviours[i].enabled = true;
        }
    }
    public void Deactivate()
    {
        for(int i = 0; i < monoBehaviours.Length; i++)
        {
            monoBehaviours[i].enabled = false;
            transform.position = initPosition;
        }
    }
}
