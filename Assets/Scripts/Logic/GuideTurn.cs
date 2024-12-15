using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideTurn : MonoBehaviour
{
    private Image image;
    public Sprite[] pages;
    private int index;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameInput.UILeft())
        {
            PrevPage();
        }
        else if(GameInput.UIRight())
        {
            NextPage();
        }
    }

    private void NextPage()
    {
        index = (index + 1) % pages.Length;
        UpdateImage();
    }
    
    private void PrevPage()
    {
        if(index < 1)
        {
            index = pages.Length - 1;
        }
        else
        {
            index--;
        }
        UpdateImage();
    }

    private void UpdateImage()
    {
        image.sprite = pages[index];
    }
}
