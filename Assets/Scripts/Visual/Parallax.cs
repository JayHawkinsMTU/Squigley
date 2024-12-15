using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject player;
    [SerializeField] float distance;
    [SerializeField] float singleTextureWidth;

    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
    }

    void SetupTexture()
    {
        if(singleTextureWidth != 0) return;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Scroll()
    {
        transform.position = new Vector3(player.transform.position.x / distance, transform.position.y, transform.position.z);      
    }

    void CheckReset()
    {
        if( (Mathf.Abs(transform.position.x) - singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
    // Update is called once per frame
    void Update()
    {   
        Scroll();
    }
}
