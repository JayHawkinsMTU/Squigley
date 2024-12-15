using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightSlash : MonoBehaviour
{
    public GameObject hazard;
    public float speed = 100;
    private float initX;
    private GameObject player;

    void Awake()
    {
        initX = hazard.transform.localPosition.x;
        player = GameObject.Find("Player");
    }

    void OnEnable()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        hazard.transform.localPosition = new Vector3(initX, 0, -1);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        hazard.transform.position += transform.right * speed * Time.deltaTime;
        if(hazard.transform.localPosition.x > 125) gameObject.SetActive(false);
    }
}
