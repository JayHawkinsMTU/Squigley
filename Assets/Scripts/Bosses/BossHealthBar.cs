using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    const float BASE_FLASH_HZ = 5; //Base frequency of flash
    [SerializeField] RectTransform bar;
    [SerializeField] TMP_Text title;
    [SerializeField] Image barImage;
    public float drainSpeed = 100; //Units per second that the healthbar drains
    private float initSizeX;
    private int maxHp = 10;
    private int curHp;
    private Color initColor;

    public void Attach(string name, int maxHp)
    {
        title.text = name;
        this.maxHp = maxHp;
        initSizeX = bar.rect.width;
        UpdateBar(maxHp);
    }

    public void Disable()
    {
        bar.sizeDelta = new Vector2(initSizeX, bar.rect.height);
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void UpdateBar(int newHp)
    {

        if(newHp > curHp)
        {
            curHp = newHp;
            bar.sizeDelta = new Vector2(initSizeX * (curHp / maxHp), bar.rect.height);
        }
        else
        {
            curHp = newHp;
            StartCoroutine(DrainToSize(initSizeX * ((float) curHp / (float) maxHp)));
        }
        IEnumerator DrainToSize(float width)
        {
            Coroutine flash = StartCoroutine(Flash(curHp - newHp * BASE_FLASH_HZ));
            while(bar.rect.width > width)
            {
                bar.sizeDelta = new Vector2(bar.rect.width - drainSpeed * Time.deltaTime, bar.rect.height);
                yield return new WaitForEndOfFrame();
            }
            curHp = newHp;
            StopCoroutine(flash);
            barImage.color = initColor;
        }
        IEnumerator Flash(float frequency)
        {
            float timeOut = 15; //Seconds to end if coroutine doesn't stop
            float time = 0;
            while(time < timeOut)
            {
                if(barImage.color == initColor)
                {
                    barImage.color = Color.white;
                }
                else
                {
                    barImage.color = initColor;
                }
                time += 1 / frequency;
                yield return new WaitForSeconds(1 / frequency);
            }
            barImage.color = initColor;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        initSizeX = bar.rect.width;
        initColor = barImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
