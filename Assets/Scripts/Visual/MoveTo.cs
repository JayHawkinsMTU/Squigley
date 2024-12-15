using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] Vector3 to;
    private Vector3 initPos;
    [SerializeField] bool globalPos = false;
    [SerializeField] bool directional = false;
    [SerializeField] float speed = 1;

    void Start()
    {
        initPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(globalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
            if(transform.position == to) enabled = false;
        } 
        else if(directional)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + to, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos + to, speed * Time.deltaTime);
            if(transform.position == initPos + to) enabled = false; 
        } 

    }
}
