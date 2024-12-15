using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class ControllerBestPractices : MonoBehaviour
{
    private bool connected = false;
    private Pause pause;

    IEnumerator CheckConditions() {
        while (true) 
        {
            // Checks controllers being connected and disconnected
            string[] controllers = Input.GetJoystickNames();

            if (!connected && (controllers.Length > 0 && controllers[0] != "")) {
                connected = true;
                if(UtilityText.primaryInstance != null) UtilityText.primaryInstance.DisplayMsg("CONTROLLER CONNECTED", Color.red);
                Connect();
            
            } else if (connected && (controllers.Length == 0 || controllers[0] == "")) {         
                connected = false;
                if(UtilityText.primaryInstance != null) UtilityText.primaryInstance.DisplayMsg("CONTROLLER DISCONNECTED", Color.red);
                Disconnect();
            }

            yield return new WaitForSeconds(1f);
        }
    }


    private void Connect()
    {
        Cursor.visible = false;
    }

    private void Disconnect()
    {
        Cursor.visible = true;
        pause.ActivatePause();
    }

    void Awake() 
    {
        pause = GetComponent<Pause>();
        StartCoroutine(CheckConditions());
    }
}
