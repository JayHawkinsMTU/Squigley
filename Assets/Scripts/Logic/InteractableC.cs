using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interactable class. Made as a newer version of the previous interactable interface blunder.
public class InteractableC : MonoBehaviour
{
    public virtual void Interact(GameObject p)
    {
        Debug.Log("Interacted by: " + p);
    }
}
