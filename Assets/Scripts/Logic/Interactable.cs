using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public abstract void Interact(GameObject p); //Should pass on player for instances where player values are changed.
}
