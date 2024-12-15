using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private const float HORIZ_DEADZONE = 0.6f; //Deadzone used for horizontal movement.
    private const float VERT_DEADZONE = 0.6f; //Deadzone used for crouching.
    private const float BIG_VERT_DEADZONE = 0.95f; //Deadzone used for jumping and fastfalling.
    public static bool tapJump = true; //Whether or not the player can jump using stick or d-pad.
    private static bool dzResetX = true;
    private static bool dzResetY = true;
    private static bool[] ffReset = {true, true}; //Index 0 - player 1. Index 1 - player 2.
    //Bindings
    //1 should correspond to wasd bindings
    //2 should correspond to arrow key bindings
    //3 should be other bindings
    //DEFAULTS
    public const string DEF_JUMP1 = "w";
    public const string DEF_JUMP2 = "up";
    public const string DEF_JUMP3 = "space";
    public const string DEF_MOVELEFT1 = "a";
    public const string DEF_MOVELEFT2 = "left";
    public const string DEF_MOVERIGHT1 = "d";
    public const string DEF_MOVERIGHT2 = "right";
    public const string DEF_CROUCH1 = "s";
    public const string DEF_CROUCH2 = "down";
    public const string DEF_INTER1 = "return";
    public const string DEF_INTER2 = "z"; 
    //Current bindings
    public static string jump1 = DEF_JUMP1;
    public static string jump2 = DEF_JUMP2;
    public static string jump3 = DEF_JUMP3;
    public static string moveleft1 = DEF_MOVELEFT1;
    public static string moveleft2 = DEF_MOVELEFT2;
    public static string moveright1 = DEF_MOVERIGHT1;
    public static string moveright2 = DEF_MOVERIGHT2;
    public static string crouch1 = DEF_CROUCH1;
    public static string crouch2 = DEF_CROUCH2;
    public static string interact1 = DEF_INTER1;
    public static string interact2 = DEF_INTER2; 

    public static string curInteract = DEF_INTER1; //Last used interact key for display

    public static void ResetBind(string action = "all")
    {
        //The big switch statement isn't ideal. Ideally I would use a hashmap or something to access all bindings.
        switch(action)
        {
            case "all":
                jump1 = DEF_JUMP1;
                jump2 = DEF_JUMP2;
                jump3 = DEF_JUMP3;
                moveleft1 = DEF_MOVELEFT1;
                moveleft2 = DEF_MOVELEFT2;
                moveright1 = DEF_MOVERIGHT1;
                moveright2 = DEF_MOVERIGHT2;
                crouch1 = DEF_CROUCH1;
                crouch2 = DEF_CROUCH2;
                interact1 = DEF_INTER1;
                interact2 = DEF_INTER2;
                break;
            case "jump1":
                jump1 = DEF_JUMP1;
                break;
            case "jump2":
                jump2 = DEF_JUMP2;
                break;
            case "jump3":
                jump3 = DEF_JUMP3;
                break;
            case "moveleft1":
                moveleft1 = DEF_MOVELEFT1;
                break;
            case "moveleft2":
                moveleft2 = DEF_MOVELEFT2;
                break;
            case "moveright1":
                moveright1 = DEF_MOVERIGHT1;
                break;
            case "moveright2":
                moveright2 = DEF_MOVERIGHT2;
                break;
            case "crouch1":
                crouch1 = DEF_CROUCH1;
                break;
            case "crouch2":
                crouch2 = DEF_CROUCH2;
                break;
            case "interact1":
                interact1 = DEF_INTER1;
                break;
            case "interact2":
                interact2 = DEF_INTER2;
                break;
            default:
                return;
        }
    }
    public static bool MoveLeft(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return (Input.GetKey(moveleft1) || Input.GetKey(moveleft2) || Input.GetAxis("Horizontal1") < -HORIZ_DEADZONE) && !(Input.GetKey(moveright1) || Input.GetKey(moveright2) || Input.GetAxis("Horizontal1") > HORIZ_DEADZONE);
            case 2:
                return Input.GetAxis("Horizontal2") < -HORIZ_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool MoveRight(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return (Input.GetKey(moveright1) || Input.GetKey(moveright2) || Input.GetAxis("Horizontal1") > HORIZ_DEADZONE) && !(Input.GetKey(moveleft1) || Input.GetKey(moveleft2) || Input.GetAxis("Horizontal1") < -HORIZ_DEADZONE);
            case 2:
                return Input.GetAxis("Horizontal2") > HORIZ_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool MoveUp(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return (Input.GetKey(jump1) || Input.GetKey(jump2) || Input.GetAxis("Vertical1") > VERT_DEADZONE);
            case 2:
                return Input.GetAxis("Vertical2") > VERT_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool MoveDown(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return (Input.GetKey(crouch1) || Input.GetKey(crouch2) || Input.GetAxis("Vertical1") < -VERT_DEADZONE);
            case 2:
                return Input.GetAxis("Vertical2") < -VERT_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool Jump(int playerID = 1)
    {
        bool returnVal = false;
        switch(playerID)
        {
            case 1:
                returnVal = Input.GetKeyDown(jump1) || Input.GetKeyDown(jump2) || Input.GetKeyDown(jump3) || Input.GetKeyDown(KeyCode.Joystick1Button0);
                if(tapJump)
                {
                    returnVal = returnVal || Input.GetAxis("Vertical1") > BIG_VERT_DEADZONE;
                }
                return returnVal;
            case 2:
                returnVal = Input.GetKeyDown("joystick 2 button 0");
                if(tapJump)
                {
                    returnVal = returnVal || Input.GetAxis("Vertical2") > BIG_VERT_DEADZONE;
                }
                return returnVal;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool Crouch(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return Input.GetKey(crouch1) || Input.GetKey(crouch2) || Input.GetAxis("Vertical1") < -VERT_DEADZONE;
            case 2:
                return Input.GetAxis("Vertical2") < -VERT_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool FastFallSimple(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                return Input.GetKeyDown(crouch1) || Input.GetKeyDown(crouch2) || Input.GetAxis("Vertical1") < -BIG_VERT_DEADZONE;
            case 2:
                return Input.GetAxis("Vertical2") < -BIG_VERT_DEADZONE;
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool FastFall(int playerID = 1)
    {
        if(ffReset[playerID - 1] && FastFallSimple(playerID))
        {
            ffReset[playerID - 1] = false;
            return true;
        }
        return false;
    }
    public static bool Interact(int playerID = 1)
    {
        switch(playerID)
        {
            case 1:
                
                return Input.GetKeyDown(interact1) || Input.GetKeyDown(interact2) || Input.GetKeyDown("joystick 1 button 2");
            case 2:
                return Input.GetKeyDown("joystick 2 button 2");
            default:
                Debug.LogError("INVALID PLAYERID");
                return false;
        }
    }
    public static bool GrappleRequest(int playerID = 1)
    {
        return Input.GetKey("joystick " + playerID + " button 3");
    }
    public static bool Pause()
    {
        return Input.GetKeyDown("escape") || Input.GetKeyDown("joystick 1 button 7");
    }
    public static bool DropIn()
    {
        return Input.GetKeyDown("joystick 2 button 7");
    }
    public static bool NeutralLeft()
    {
        return MoveLeft(1) || MoveLeft(2);
    }
    public static bool NeutralRight()
    {
        return MoveRight(1) || MoveRight(2);
    }
    public static bool NeutralUp()
    {
        return MoveUp(1) || MoveUp(2);
    }
    public static bool NeutralDown()
    {
        return MoveDown(1) || MoveDown(2);
    }
    public static bool UIUp()
    {
        if(NeutralUp())
        {
            if(dzResetY)
            {
                dzResetY = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool UIDown()
    {
        if(NeutralDown())
        {
            if(dzResetY)
            {
                dzResetY = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool UILeft()
    {
        if(NeutralLeft())
        {
            if(dzResetX)
            {
                dzResetX = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool UIRight()
    {
        if(NeutralRight())
        {
            if(dzResetX)
            {
                dzResetX = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool NoclipFlat()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    public static bool NoclipLock()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
    private void DeadzoneReset()
    {
        if(!NeutralLeft() && !NeutralRight())
        {
            dzResetX = true;
        }
        if(!NeutralUp() && !NeutralDown())
        {
            dzResetY = true;
        }
        if(!FastFallSimple(1))
        {
            ffReset[0] = true;
        }
        if(!FastFallSimple(2))
        {
            ffReset[1] = true;
        }
    }
    //Script should be attached to event system in every scene.
    void Update()
    {
        DeadzoneReset();
        //Updates curInteract
        if(Input.GetKeyDown("joystick 1 button 2"))
        {
            curInteract = "x";
        }
        else if(Input.GetKeyDown(interact2))
        {
            curInteract = interact2;
        }
        else if(Input.GetKeyDown(interact1))
        {
            curInteract = interact1;
        }
    }
    void LoadData(SaveData data)
    {
        jump1 = data.jump1;
        jump2 = data.jump2;
        jump3 = data.jump3;
        moveleft1 = data.moveleft1;
        moveleft2 = data.moveleft2;
        moveright1 = data.moveright1;
        moveright2 = data.moveright2;
        crouch1 = data.crouch1;
        crouch2 = data.crouch2;
        interact1 = data.interact1;
        interact2 = data.interact2;
    } 
    void SaveData(ref SaveData data)
    {
        data.jump1  =jump1;
        data.jump2 = jump2;
        data.jump3 = jump3;
        data.moveleft1 = moveleft1;
        data.moveleft2 = moveleft2;
        data.moveright1 = moveright1;
        data.moveright2 = moveright2;
        data.crouch1 = crouch1;
        data.crouch2 = crouch2;
        data.interact1 = interact1;
        data.interact2 = interact2;
    }
}
