using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(projectilePrefab, new Vector3(transform.position.x + 0.5f, transform.position.y, -1), Quaternion.identity).GetComponent<Projectile>().Fire(5, 0);
    }
}
