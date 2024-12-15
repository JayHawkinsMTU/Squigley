using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    private GameObject currentProjectile;
    [SerializeField] float cooldown = 3;
    [SerializeField] float projectileVelocity = 3f;
    [SerializeField] float rotation = 0;
    [SerializeField] bool waitForDestroy = true;
    [SerializeField] bool independentProjectileRotation = false;
    [SerializeField] bool toPlayer = false;
    [SerializeField] bool fromCenter = false;
    private GameObject player;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip warnSound;
    private bool active;
    private Sprite initialSprite;
    [SerializeField] Sprite prepSprite;
    SpriteRenderer spi;

    // Start is called before the first frame update
    void Start()
    {
        spi = GetComponent<SpriteRenderer>();
        initialSprite = spi.sprite;
        if(toPlayer) player = GameObject.Find("Player");
    }
    IEnumerator Shoot()
    {
        
        active = true;
        yield return new WaitForSeconds(cooldown);
        if(prepSprite != null)
        {
            spi.sprite = prepSprite;
            audioSource.PlayOneShot(warnSound, 0.6f);
            yield return new WaitForSeconds(1);
        }
        audioSource.PlayOneShot(shootSound, 0.7f);
        spi.sprite = initialSprite;
        if(fromCenter)
        {
            currentProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        }
        else
        {
            currentProjectile = Instantiate(projectilePrefab, new Vector3(transform.position.x + Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), transform.position.y + Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) +.125f, transform.position.z), Quaternion.identity) as GameObject;
        }
        float angle = transform.eulerAngles.z;
        if(independentProjectileRotation)
        {
            angle = rotation;
        }
        if(toPlayer)
        {
            angle = Vector2.SignedAngle(Vector2.right, player.transform.position - transform.position);
        }
        currentProjectile.GetComponent<Projectile>().Fire(projectileVelocity * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),  projectileVelocity * Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), angle);
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
        {
            if(!waitForDestroy || currentProjectile == null) StartCoroutine(Shoot());
        }
    }
}
