using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionMatrix : ButtonMatrix
{
    public int price = 0;
    public Decision decision;
    public TMP_Text title;
    [SerializeField] Pause pause;
    public void Open()
    {
        pause.UtilityPause();
    }
    public void Close()
    {
        pause.UtilityUnpause();
        transform.parent.gameObject.SetActive(false);
    }

}