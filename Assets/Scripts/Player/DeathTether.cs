using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTether : MonoBehaviour
{
    GameObject player1;
    [SerializeField] float maxDistance = 50;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player1.transform.position) > maxDistance)
        {
            GetComponent<SecondaryPlayerDeath>().Die();
        }
    }
}
