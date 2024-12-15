using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    private GameObject player;
    [SerializeField] GameObject iris;
    public bool lookAway;

    void Follow(Vector2 to)
    {
        float radius = .42f;
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2) transform.position - to);
        if(!lookAway)
        {
            radius = (iris.transform.localScale.x / 2) * Mathf.Clamp(Vector2.Distance(transform.position, to) / (transform.localScale.x * 2) , 0, 1) - .08f;
            angle -= 180;
        }
        iris.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius, iris.transform.localPosition.z);
    }
    void OnEnable()
    {
        iris.transform.localPosition = new Vector3(0, 0, iris.transform.localPosition.z);
    }
    void Awake()
    {
        player = GameObject.Find("Player");
        transform.eulerAngles = Vector3.zero;
    }
    void Update()
    {
        Follow(player.transform.position);
    }
}
