using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] float z = 2;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        line.SetPosition(0, new Vector3(point1.transform.position.x, point1.transform.position.y, z));
        line.SetPosition(1, new Vector3(point2.transform.position.x, point2.transform.position.y, z));

    }
}
