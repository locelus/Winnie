using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitscan : EnemyWeapon {

    public Transform bulletTrailPrefab;
    public float effectSpawnRate;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;
    float timeToSpawnEffect;

    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }


    public override void Shoot() {
        base.Shoot();
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, shootPosition - firePointPosition, 100, whatToHit);
        if (hit.collider != null) {
            Player player = hit.collider.GetComponent<Player>();
            if (player != null) {
                player.DamagePlayer(damage);
            }
        }
        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) {
                hitPos = (shootPosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal) {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null) {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.05f);

        if (hitNormal != new Vector3(9999, 9999, 9999)) {
            Transform hitParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint) as Transform;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, 0);
        Destroy(clone.gameObject, 0.02f);
    }
}
