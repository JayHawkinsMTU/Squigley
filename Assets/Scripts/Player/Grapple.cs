using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grapple : MonoBehaviour
{
    TMP_Text text;
    int playerID;
    bool grounded;
    GameObject player;
    MultiplayerHandler multiplayer;
    Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent.gameObject;
        multiplayer = GameObject.Find("EventSystem").GetComponent<MultiplayerHandler>();
        movement = player.gameObject.GetComponent<Movement>();
        playerID = movement.playerID;
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = movement.grounded;
        if(GameInput.GrappleRequest(playerID)) //Y
        {
            text.enabled = true;
            multiplayer.req[playerID -1] = true;
            /*if(grounded) multiplayer.groundedPlayer = player;
            else multiplayer.airbournePlayer = player;*/
        }
        else
        {
            text.enabled = false;
            multiplayer.req[playerID -1] = false;
            /*multiplayer.groundedPlayer = null;
            multiplayer.airbournePlayer = null;*/
        }
    }
}
