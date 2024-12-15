using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parryable : MonoBehaviour
{
    public virtual void Parry()
    {
        Debug.Log("Parried " + this.gameObject + "!");
    }
}
