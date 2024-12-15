using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : Box
{
    [SerializeField] bool unlimited;
    [SerializeField] GameObject projectilethrow; //projectile holder
    public override void Interact(GameObject p)
    {
        Instantiate(projectilethrow, p.transform).GetComponent<ProjectileThrow>().Enable(p);
        if(!unlimited)
        {
            Destroy(this.gameObject);
        }
    }
}
