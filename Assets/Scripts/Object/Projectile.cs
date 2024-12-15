using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Parryable
{
    [SerializeField] bool directedSprite;
    [SerializeField] bool spin;
    [SerializeField] bool baseOnAngle;
    [SerializeField] bool stick;
    [SerializeField] GameObject particles; //Prefab of particles upon collision.
    [SerializeField] float timeOut = 60; // Destroy after 60 seconds
    [SerializeField] float parryMultiplier = -2f; // The number that the velocity gets multiplied by upon parry.
    Rigidbody2D rb;
    SpriteRenderer spi;
    public void Fire(float velX, float velY, float rotation = 0) //Fire straight left or right.
    {
        if(directedSprite && velX < 0 && rotation == 0)
        {
            spi.flipX = true;
        }
        else
        {
            spi.flipX = false;
        }
        transform.eulerAngles = new Vector3(0, 0, rotation); //Sets rotation
        if(baseOnAngle)
        {
            rb.velocity = transform.right * velX;
        }
        else
        {
            rb.velocity = new Vector2(velX, velY);
        }
        if(spin)
        {
            float angVel = Random.Range(-500, -700);
            angVel *= Mathf.Sign(velX);
            rb.angularVelocity = angVel;
        }
    }
    public override void Parry()
    {
        Fire(rb.velocity.x * parryMultiplier, rb.velocity.y * parryMultiplier); //Reflects projectile
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(stick) Instantiate(particles, collision.GetContact(0).point, transform.rotation);
        else if(particles != null) Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    void Awake()
    {
        spi = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(timeOut);
        Destroy(this.gameObject);
    }
}
