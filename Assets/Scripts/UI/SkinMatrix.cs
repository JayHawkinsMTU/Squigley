using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinMatrix : ButtonMatrix
{
    public Pause pause;
    public GameObject player;
    public virtual void Open(GameObject p)
    {
        player = p;
        pause.UtilityPause();
        ToButton(startX, startY);
    }
    public virtual void Close()
    {
        pause.UtilityUnpause();
        transform.parent.gameObject.SetActive(false);
    }
}
