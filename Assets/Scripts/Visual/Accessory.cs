using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : MonoBehaviour
{
    public string accessoryName = "NONAME";
    public int price = 10;
    public Sprite sprite;
    public Color color = Color.white;
    public bool matchPlayerColor = false;
    public bool mathPlayerOrientation = false;
    public Vector3 offset = new Vector3(0, 0, -.5f);
    public Vector2 scale = new Vector2(1, 1);
}
