using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompt : MonoBehaviour
{
    private const float A_PER_SECOND = 1.25f;
    private const float UNITS_PER_SECOND = 2f;
    private const float FREQUENCY = 0.4f;
    private const float MAGNITUDE = 0.115f;
    private TMP_Text text;
    private float height = 1f;
    private Vector3 initPos;
    private float target;
    public void Open()
    {
        initPos = transform.position;
        text = GetComponent<TMP_Text>();
        text.text = "[" + GameInput.curInteract + "]";
        StopAllCoroutines();
        StartCoroutine(OpenAnimation());

        IEnumerator OpenAnimation()
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            //Appear
            while(text.color.a < 1 || height - transform.localPosition.y > 0)
            {
                yield return new WaitForEndOfFrame();
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + A_PER_SECOND * Time.deltaTime);
                transform.Translate((target - transform.position.y) * Time.deltaTime * UNITS_PER_SECOND * Vector2.up);
            }
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

            float time = 0;
            float topY = transform.position.y;
            //Sway
            while(true)
            {
                transform.position = new Vector3(transform.position.x, topY + MAGNITUDE * Mathf.Sin(time * FREQUENCY * 2 * Mathf.PI), transform.position.z);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void Close()
    {
        StopAllCoroutines();
        if(enabled) StartCoroutine(CloseAnimation());

        IEnumerator CloseAnimation()
        {
            while(transform.position.y > initPos.y + .001f || text.color.a > 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - A_PER_SECOND * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, initPos, Vector2.Distance(transform.position, initPos) * UNITS_PER_SECOND * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        target = transform.position.y + height;
    }

    void Update()
    {
        if(text != null)
        {
            text.text = "[" + GameInput.curInteract + "]";
        }
        if(GameInput.Interact(1) || GameInput.Interact(2))
        {
            Close();
        }
    }
}
