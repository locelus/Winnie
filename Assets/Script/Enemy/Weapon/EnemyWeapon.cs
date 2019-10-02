using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWeapon : MonoBehaviour {
    public bool semiAuto;
    public float fireRate;
    public float damage;
    public LayerMask whatToHit;
    public bool seen;

    [HideInInspector]
    public Vector2 shootPosition;
    [HideInInspector]
    public Vector2 firePointPosition;
    [HideInInspector]
    public Transform firePoint;
    float timeToFire;


    // Use this for initialization
    public virtual void Start() {
        firePoint = transform.Find("Firepoint");
        if (firePoint == null) {
            Debug.LogError("Firepoint is null");
        }
        shootPosition = new Vector2(-999999, 0);
        
    }

    public virtual void Update() {
        if (Time.timeScale != 0 && seen) {
            if (semiAuto == true) {
                if (Time.time > timeToFire) {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot();
                }
            }
            else {
                if (Time.time > timeToFire) {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot();
                }
            }

        }
    }

    public virtual void Shoot() {
        firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
    }
}
