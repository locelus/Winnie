using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //If applicable, delay for explosion
    [Header("Set delay to -32678 if you don't want it to explode after the countdowns")]
    public float delay = 3f;
    //Blast radius, in world units
    public float blastRadius = 5f;
    //Knockback force
    public float force = 700f;
    //Damage done by direct impact, scales down by distance
    public float damage = 30f;
    //Amount of movement force the explosion gives to items around it
    public float movementForce = 50f;
    //Storer for calculations
    float calculatedForce;
    //Where the player throws the item
    Vector2 throwVector;
    //public GameObject arm;
    //public ArmRotation armRotation;
    //Amount of time the explosion stays around after it's created
    public float timeToDestroyExplosion;
    public bool explodeOnImpact;
    public bool explodesAfterCountdown;
    public bool throwable;
    public bool sticky;
    public bool arrow;

    Vector3 mousePosition;

    Collider2D playerCollider;
    Collider2D localCollider;
    public GameObject explosionEffect;
    Rigidbody2D objectRigidbody;
    float countdown;
    GameObject explosion;
    GameObject player;

    //public KeyCode grenadeCode;
    bool alreadyThrown = false;

    // Use this for initialization
    void Start() {
        localCollider = GetComponent<Collider2D>();
        objectRigidbody = GetComponent<Rigidbody2D>();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerCollider);
        countdown = delay;
        if (!throwable) {
            throwVector = mousePosition - transform.position;
            calculatedForce = movementForce / throwVector.magnitude;
            objectRigidbody.AddForce(throwVector * calculatedForce);
        }
        StartCoroutine(DestroyProjectile(30));
    }

    public void Throw() {
        transform.parent = null;
        throwVector = mousePosition - transform.position;
        objectRigidbody.simulated = true;
        calculatedForce = movementForce / throwVector.magnitude;
        objectRigidbody.AddForce(throwVector * calculatedForce);
    }
    

    public void OnCollisionEnter2D(Collision2D collision) {
        if (sticky && (collision.gameObject.GetComponent<Projectile>() == null)) {
            objectRigidbody.bodyType = RigidbodyType2D.Static;
            this.localCollider.enabled = false;
            StartCoroutine(DestroyProjectile(5));
        }
        if (!explodeOnImpact) {
            return;
        }
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            Explode(enemy);
        }
        Explode(null);
    }

    // Update is called once per frame
    void Update() {
        if (arrow && (objectRigidbody.bodyType != RigidbodyType2D.Static)) {
            Vector2 v = objectRigidbody.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        countdown -= Time.deltaTime;
        if (countdown <= 0f && explodesAfterCountdown) {
            Explode(null);
        }
    }

    void Explode(Enemy hitEnemy) {
        //Show explosion effect
        explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, timeToDestroyExplosion);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach (Collider2D nearbyObject in colliders) {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (enemy != null) {
                Vector2 damageDir = (rb.transform.position - explosion.transform.position);
                float wearoff = 1 - (damageDir.magnitude / blastRadius);
                enemy.DamageEnemy(damage * wearoff);
            }
            if (rb != null) { //Copied from https://forum.unity.com/threads/need-rigidbody2d-addexplosionforce.212173/
                Vector2 rbDir = (rb.transform.position - explosion.transform.position);
                float wearoff = 1 - (rbDir.magnitude / blastRadius);
                rb.AddForce(rbDir.normalized * force * wearoff);
            }
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyProjectile(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}