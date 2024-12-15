using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    public AudioClip activationSound;
    public virtual void Activate()
    {
        //Debug.Log("Activated");
    }
    public virtual void Hover()
    {
        if(GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().color = Color.yellow;
        }
    }
    public virtual void Unhover()
    {
        if(GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().color = Color.white;
        }
    }
}
