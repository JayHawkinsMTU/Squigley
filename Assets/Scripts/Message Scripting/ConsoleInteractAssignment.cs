using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInteractAssignment : MonoBehaviour
{
    [SerializeField] MonoBehaviour interactable;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerInteractKey>().interactable = (Interactable) interactable;
    }
}
