using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractKey : MonoBehaviour
{
    public Interactable interactable;
    private int playerID;
    [SerializeField] Pause pause;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Movement>() != null)
        {
            playerID = GetComponent<Movement>().playerID;
        }
        else
        {
            playerID = 1;
        }
        GameObject eventSystem = GameObject.Find("EventSystem");
        if(eventSystem != null)
        {
            interactable = eventSystem.GetComponent<NullInteractable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((pause == null || !pause.paused) && (interactable != null && Input.GetKeyDown("z") || Input.GetKeyDown("return") || Input.GetKeyDown("joystick " + playerID + " button 2")))
        {
            interactable.Interact(this.gameObject);
        }
    }
}
