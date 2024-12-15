using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAfterDelay : MonoBehaviour
{
    [SerializeField] float delay = 30; //Time until fade starts in seconds.
    [SerializeField] float fadeTime = 5; //Time it takes for the image to fade.
    [SerializeField] [Range(0f,1f)] float fadeToOpacity = 0; //Opacity the image will fade to.
    [SerializeField] Image image; //The image to fade.
    [SerializeField] bool fadeIn = false;
    Wait wait;

    void Start()
    {
        wait = new Wait(delay);
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        wait.Iterate();
        if(wait.isDone())
        {
            if(!fadeIn)
            {
                if(image.color.a > fadeToOpacity)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (fadeToOpacity * Time.deltaTime) / fadeTime);
                }
                if(image.color.a <= fadeToOpacity)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, fadeToOpacity);
                    enabled = false;
                }
            }
            else
            {
                if(image.color.a < fadeToOpacity)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (fadeToOpacity * Time.deltaTime) / fadeTime);
                }
                if(image.color.a >= fadeToOpacity)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, fadeToOpacity);
                    enabled = false;
                }
            }
        }
    }
}
