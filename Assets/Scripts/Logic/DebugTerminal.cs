using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DebugTerminal : MonoBehaviour, IDataPersistence
{
    public static bool debug;
    private bool active;
    private Pause pause;
    [SerializeField] bool voidScene = false;
    [SerializeField] UtilityText utilityText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject player1;
    [SerializeField] followDaGuy playerCamera;
    [SerializeField] DataPersistenceManager dpm;
    PlayerInteractKey pik;
    
    //Undo
    bool undoing = false;
    private Stack<string> undoStack = new Stack<string>();
    private Stack<string> redoStack = new Stack<string>();
    //Editor
    public GameObject[] prefabs;
    static Dictionary<string, int> objDict;
    string[] objNames = {"whiteplat", "greyplat", "recthaz", "spike", "rotabox", "latchbox", "slugfield", "spdfield", "rubble", "ice","goopblock","coin", "jumpcoin", "checkpoint", "finish"};
    [SerializeField] GameObject addedObjects;
    void OpenTerminal()
    {
        utilityText.CloseMsg();
        pik.enabled = false;
        inputField.enabled = true;
        inputField.ActivateInputField();
        pause.UtilityPause();
        inputField.text = ">";
        inputField.caretPosition = 1;
        active = true;
    }
    void CloseTerminal()
    {
        inputField.text = "";
        if(pause.paused)
        {
            pause.UtilityUnpause();
        }
        inputField.DeactivateInputField();
        inputField.enabled = false;
        pik.enabled = true;
        active = false;
    }
    public void EnterCmd(string cmd, bool undo = false)
    {
        cmd = cmd.ToLower();
        string[] args = cmd.Split(' ');
        undoing = undo;
        try
        {
            if(args[0] != ">undo" && args[0] != ">redo")
            {
                redoStack.Clear();
            }
            switch(args[0])
            {
                case ">debug":
                    if(args.Length == 1)
                    {
                        DisplayDebug();
                    }
                    else
                    {
                        SetDebug(args[1]);
                    }
                    break;
                case ">pos":
                    if(args.Length == 1)
                        GetPosition();
                    else
                        SetPosition(args[1], args[2]);
                    break;
                case ">smoothcam":
                    if(args.Length == 1)
                    {
                        utilityText.DisplayMsg("Invalid args. Do: \">smoothcam [true/false]\"", Color.red);
                        break;
                    }
                    SetSmoothcam(args[1]);
                    break;
                case ">coin":
                    SetCoins(args[1]);
                    break;
                case ">lives":
                    SetLives(args[1]);
                    break;
                case ">noclip":
                    ToggleNoclip();
                    break;
                case ">time":
                    SetTimeScale(args[1]);
                    break;
                case ">invis":
                    if(args.Length != 2)
                    {
                        SetInvis();
                    }
                    else
                    {
                        SetInvis(args[1]);
                    }
                    break;
                case ">obj":
                    if(args.Length == 4)
                    {
                        Object(args[1], args[2], args[3]);
                    }
                    else if(args.Length == 6)
                    {
                        Object(args[1], args[2], args[3], args[4], args[5]);
                    }
                    else
                    {
                        Object(args[1], args[2], args[3], args[4], args[5], args[6]);
                    }
                    break;
                case ">des":
                    Des(args[1], args[2]);
                    break;
                case ">bind":
                    if(args.Length != 3)
                    {
                        InvalidArg();
                    }
                    else
                    {
                        Bind(args[1], args[2]);
                    }
                    break;
                case ">undo":
                    if(args.Length == 1)
                    {
                        UndoRedo("1", true);
                    }
                    else
                    {
                        UndoRedo(args[1], true);
                    }
                    break;
                case ">redo":
                    if(args.Length == 1)
                    {
                        UndoRedo("1", false);
                    }
                    else
                    {
                        UndoRedo(args[1], false);
                    }
                    break;
                case ">void":
                    dpm.SaveGame();
                    SceneManager.LoadScene("Void");
                    break;
                case ">overworld":
                    SceneManager.LoadScene("Overworld");
                    break;
                case ">load":
                    if(!voidScene) break;
                    MapLoader loader = GetComponent<MapLoader>();
                    loader.Load(args[1]);
                    this.addedObjects = loader.addedObjects; //Assigns reference to new added objects for new objects to be children of.
                    break;
                case ">save":
                    if(!voidScene)
                    {
                        dpm.SaveGame();
                        return;
                    }
                    if(args.Length == 1)
                    {
                        GetComponent<MapLoader>().Save();
                    }
                    else
                    {
                        GetComponent<MapLoader>().SaveAs(args[1]);
                    } 
                    break;
                default:
                    InvalidCmd(args[0]);
                    break;
            }
        } 
        finally
        {
            CloseTerminal();
        }
    }
    void InvalidArg(string msg = "COMMAND ARGUMENT INVALID")
    {
        utilityText.DisplayMsg(msg, Color.red);
        GetComponent<NullInteractable>().Interact(this.gameObject);
    }
    private bool DebugRequired()
    {
        if(debug || voidScene) return false; //Debug is allowed locally in void scenes
        utilityText.DisplayMsg("DEBUG MODE REQUIRED. DO \">debug true\"", Color.red);
        GetComponent<NullInteractable>().Interact(this.gameObject);
        return true;
    }
    void InvalidCmd(string cmd)
    {
        if(cmd[0] == '>') //Message can be ignored if sign is removed.
        {
            utilityText.DisplayMsg("COMMAND INVALID", Color.red);
            GetComponent<NullInteractable>().Interact(this.gameObject);
        }
    }
    //Uses "~" to signify a change in coords.
    private Vector3 TildeCoords(string arg1, string arg2, string arg3 = "~0")
    {
        float x;
        float y;
        float z;
        if(arg1[0] == '~')
        {
            x = float.Parse(arg1.Substring(1)) + player1.transform.position.x;
        }
        else
        {
            x = float.Parse(arg1);
        }
        if(arg2[0] == '~')
        {
            y = float.Parse(arg2.Substring(1)) + player1.transform.position.y;
        }
        else
        {
            y = float.Parse(arg2);
        }
        if(arg3[0] == '~')
        {
            z = float.Parse(arg3.Substring(1)) + player1.transform.position.z;
        }
        else
        {
            z = float.Parse(arg3);
        }
        return new Vector3(x, y, z);
    }
    void Stackdo(string cmd)
    {

        if(!undoing)
        {
            //Debug.Log("added" + cmd + "to undostack");
            undoStack.Push(cmd);
        }
        else
        {
            //Debug.Log("added" + cmd + "to redostack");
            redoStack.Push(cmd);
        }
    }
    //Commands
    void SetDebug(string arg1)
    {
        if(debug)
        {
            InvalidArg("DEBUG CANNOT BE CHANGED ONCE TRUE UNLESS SAVE DATA IS RESET");
            return;
        }
        try
        {
            debug = bool.Parse(arg1);
            if(debug)
            {
                utilityText.DisplayMsg("DEBUG MODE ENABLED, ACHIEVEMENTS OFF UNTIL SAVE DATA RESET", Color.green);
                dpm.SaveGame();
            }
        }
        catch
        {
            InvalidArg();
        }
    }
    void DisplayDebug()
    {
        utilityText.DisplayMsg("DEBUG IS SET TO: " + debug, Color.green);

    }
    void SetSmoothcam(string arg1) //Undoable
    {
        try
        {
            Stackdo(">smoothcam " + !bool.Parse(arg1));
            playerCamera.smoothCamera = bool.Parse(arg1);
        }
        catch
        {
            InvalidArg();
        }
    }
    void SetPosition(string arg1, string arg2) //Undoable
    {
        if(DebugRequired())
        {
            return;
        }
        try
        {
            Vector3 oldPos = player1.transform.position;
            Stackdo(">pos " + oldPos.x + " " + oldPos.y + " " + oldPos.z);
            //player1.transform.position = new Vector3(float.Parse(arg1), float.Parse(arg2), player1.transform.position.z);
            player1.transform.position = TildeCoords(arg1, arg2);
        }
        catch
        {
            InvalidArg();
        }
    }
    void GetPosition()
    {
        utilityText.DisplayMsg("X: " + player1.transform.position.x + " Y: " + player1.transform.position.y, Color.gray);
    }
    void SetCoins(string arg1) //Undoable
    {
        if(DebugRequired())
        {
            return;
        }
        try
        {
            int oldCoinCount = GetComponent<UICoinHandler>().coinCount;
            Stackdo(">coin " + oldCoinCount);

            GetComponent<UICoinHandler>().coinCount = int.Parse(arg1);
        }
        catch
        {
            InvalidArg();
        }
    }
    void SetLives(string arg1) //Undoable
    {
        if(DebugRequired())
        {
            return;
        }
        try
        {
            Stackdo(">lives " + GetComponent<UICoinHandler>().checkPointLives);
            GetComponent<UICoinHandler>().checkPointLives = int.Parse(arg1);
            player1.GetComponent<Movement>().checkPointLives = int.Parse(arg1);
        }
        catch
        {
            InvalidArg();
        }
    }
    void ToggleNoclip()
    {
        if(DebugRequired())
        {
            return;
        }
        else
        {
            player1.GetComponent<NoclipMovement>().ToggleNoclip();
        }
    }
    void SetTimeScale(string arg1) //Undoable
    {
        if(DebugRequired())
        {
            return;
        }
        try
        {
            Stackdo(">time " + Time.timeScale);
            float newTimeScale = float.Parse(arg1);
            pause.UtilityUnpause();
            if(newTimeScale <= 10)
            {
                Time.timeScale = float.Parse(arg1);
            }
        }
        catch
        {
            InvalidArg();
        }
    }
    void UndoRedo(string arg1, bool undo) //Undo if true, redo if false
    {
        int iterations = int.Parse(arg1);
        if(undo)
        {
            //Debug.Log("undoing " + iterations);
            for(int i = 0; i < iterations; i++)
            {
                if(undoStack.Count == 0) {
                    utilityText.DisplayMsg("Nothing left in undo stack", Color.gray);
                    break;
                } 
                EnterCmd(undoStack.Pop(), undo);
                //Debug.Log(undoStack.Pop());
            }
        }
        else //Redo
        {
            //Debug.Log("redoing " + iterations);

            for(int i = 0; i < iterations; i++)
            {
                if(redoStack.Count == 0) {
                    utilityText.DisplayMsg("Nothing left in redo stack", Color.gray);
                    break;
                }
                EnterCmd(redoStack.Pop(), undo);
                //Debug.Log(undoStack.Pop());
            }
        }
        
    }
    void Bind(string arg1, string arg2)
    {
        switch(arg1)
        {
            case "reset":
                GameInput.ResetBind(arg2);
                break;
            case "jump1":
                GameInput.jump1 = arg2;
                break;
            case "jump2":
                GameInput.jump2 = arg2;
                break;
            case "jump3":
                GameInput.jump3 = arg2;
                break;
            case "moveleft1":
                GameInput.moveleft1 = arg2;
                break;
            case "moveleft2":
                GameInput.moveleft2 = arg2;
                break;
            case "moveright1":
                GameInput.moveright1 = arg2;
                break;
            case "moveright2":
                GameInput.moveright2 = arg2;
                break;
            case "crouch1":
                GameInput.crouch1 = arg2;
                break;
            case "crouch2":
                GameInput.crouch2 = arg2;
                break;
            case "interact1":
                GameInput.interact1 = arg2;
                break;
            case "interact2":
                GameInput.interact2 = arg2;
                break;
        }
    }
    void SetInvis(string arg1 = "")
    {
        SpriteRenderer spi = player1.GetComponent<SpriteRenderer>();
        if(arg1 == "")
        {
            spi.enabled = !spi.enabled;
        }
        else
        {
            spi.enabled = bool.Parse(arg1);
        }
    }
    /*arg1 object ID
    arg2 posX
    arg3 posY
    arg4 sizeX
    arg5 sizeY
    arg6 rotation
    */
    void Object(string arg1, string arg2, string arg3, string arg4 = "1", string arg5 ="1", string arg6 = "0")
    {
        if(DebugRequired())
        {
            return;
        }
        int index;
        try
        {
            index = int.Parse(arg1); //If number is input, directly use number.
        }
        catch
        {
            //Use hashmap otherwise.
            if(objDict.ContainsKey(arg1))
            {
                index = objDict[arg1];
            }
            else
            {
                InvalidArg("Invalid object ID");
                return;
            }
        }
        if(index < 0 || index >= prefabs.Length)
        {
            InvalidArg("Invalid object index");
            return;
        }
        else
        {
            Vector2 coords = TildeCoords(arg2, arg3);
            float sizeX = float.Parse(arg4);
            float sizeY = float.Parse(arg5);
            float rotation = float.Parse(arg6);
            GameObject obj = Instantiate(prefabs[index], coords, Quaternion.identity) as GameObject;
            obj.transform.localScale = new Vector2(sizeX, sizeY);
            obj.transform.eulerAngles = new Vector3(0, 0, rotation);
            obj.transform.parent = addedObjects.transform;
            Stackdo(">des " + coords.x.ToString() + " " + coords.y.ToString());
        }
    }
    void Des(string arg1, string arg2, string arg3 = "0.01", string arg4 = "0.01")
    {
        if(DebugRequired())
        {
            return;
        }
        Vector2 coords = TildeCoords(arg1, arg2);
        float sizeX = float.Parse(arg3);
        float sizeY = float.Parse(arg4);
        RaycastHit2D cast = Physics2D.BoxCast(coords, new Vector2(sizeX, sizeY), 0, Vector2.zero, 0);
        GameObject dO = cast.collider.gameObject;
        Stackdo(">obj " + dO.name[0] + " " + dO.transform.position.x + " " + dO.transform.position.y + " " + dO.transform.localScale.x + " " + dO.transform.localScale.y + " " + dO.transform.eulerAngles.z);
        Destroy(dO);
    }

    void ConstructDict()
    {
        if(objDict == null)
        {
            objDict = new Dictionary<string, int>();
            for(int i = 0; i < objNames.Length; i++)
            {
                objDict.Add(objNames[i], i);
            }
        }
    }
    void Start()
    {
        pause = GetComponent<Pause>();
        pik = player1.GetComponent<PlayerInteractKey>();
        if(playerCamera == null) playerCamera = GameObject.Find("Player Camera").GetComponent<followDaGuy>(); 
        ConstructDict();
    }
    // Update is called once per frame
    void Update()
    {
        if(!active)
        {
            if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.Period))
            {
                OpenTerminal();
            }
        }
        else
        {
            if(Input.GetKey(KeyCode.Return))
            {
                EnterCmd(inputField.text);
            }
            if(Input.GetKey(KeyCode.Escape))
            {
                CloseTerminal();
            }
        }
    }
    public void SaveData(ref SaveData data)
    {
        if(debug) //Can only be set to true in data.
        {
            data.debug = true;
        }
    }
    public void LoadData(SaveData data)
    {
        debug = data.debug;
    }
}
