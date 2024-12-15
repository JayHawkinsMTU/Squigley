using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorMatrix : SkinMatrix
{
    [SerializeField] Image colorPreview;
    [SerializeField] Image skinPreview;
    [SerializeField] TMP_Text priceDisplay;
    [SerializeField] UICoinHandler coinHandler;
    [SerializeField] AudioClip cantBuy;
    [SerializeField] TMP_Text rDisplay, gDisplay, bDisplay;
    ColorHandler colorHandler;
    Color playerColor;
    int price = 0;
    public float r;
    public float g;
    public float b;
    public override void Open(GameObject p)
    {
        player = p;
        pause.UtilityPause();
        ToButton(startX, startY);
        GetColor();
        Start();
    }
    private void GetColor()
    {
        r = playerColor.r;
        g = playerColor.g;
        b = playerColor.b;
    }
    // Start is called before the first frame update
    void Start()
    {
        colorHandler = player.GetComponent<ColorHandler>();
        playerColor = player.GetComponent<SpriteRenderer>().color;
        skinPreview.sprite = player.GetComponent<SpriteRenderer>().sprite;
        GetColor();
        rDisplay.SetText("Red : " + ((int) (r * 10)).ToString());
        gDisplay.SetText("Green : " + ((int) (g * 10)).ToString());
        bDisplay.SetText("Blue : " + ((int) (b * 10)).ToString());
    }
    public void BumpComponent(char comp, float diff)
    {
        switch(comp)
        {
            case 'r':
                r = Mathf.Clamp(r + diff, 0, 1);
                rDisplay.SetText("Red : " + (Mathf.Round(r * 10)).ToString());
                break;
            case 'g':
                g = Mathf.Clamp(g + diff, 0, 1);
                gDisplay.SetText("Green : " + (Mathf.Round(g * 10)).ToString());
                break;
            case 'b':
                b = Mathf.Clamp(b + diff, 0, 1);
                bDisplay.SetText("Blue : " + (Mathf.Round(b * 10)).ToString());
                break;
            default:
                Debug.LogError("Invalid component in BumpComponent");
                break;
        }
        price = (int) (10 * (Mathf.Abs(playerColor.r - r) + Mathf.Abs(playerColor.g - g) + Mathf.Abs(playerColor.b - b)));
        colorPreview.color = new Color(r, g, b);
        skinPreview.color = new Color(r, g, b);
        priceDisplay.SetText("Cost: " + price.ToString());

    }
    public override void Close()
    {
        pause.UtilityUnpause();
        playerColor = player.GetComponent<SpriteRenderer>().color;
        
        if(coinHandler.coinCount >= price)
        {
            coinHandler.coinCount -= price;
            colorHandler.ChangeColor(new Color(r, g, b), true);
        }
        else
        {
            audioSource.PlayOneShot(cantBuy, 0.75f);
        }
        r = playerColor.r;
        g = playerColor.g;
        b = playerColor.b;
        Navigate('E');
        rDisplay.SetText("Red : " + ((int) (r * 10)).ToString());
        gDisplay.SetText("Green : " + ((int) (g * 10)).ToString());
        bDisplay.SetText("Blue : " + ((int) (b * 10)).ToString());
        transform.parent.gameObject.SetActive(false);
    }
}
