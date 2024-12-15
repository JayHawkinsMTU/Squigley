using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpkpAnimationTriggers : MonoBehaviour
{
    public GameObject shopKeep;
    [SerializeField] string currentAnimation;
    [SerializeField] string secondaryAnimation;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            shopKeep.GetComponent<ShopKeeperAnimation>().currentAnimation = currentAnimation;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            shopKeep.GetComponent<ShopKeeperAnimation>().currentAnimation = secondaryAnimation;
        }
    }
}
