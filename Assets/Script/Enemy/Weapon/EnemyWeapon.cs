using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWeapon : Firearm {
    public Transform bulletTrailPrefab;
    public float effectSpawnRate;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;
    float timeToSpawnEffect;
    public Vector2 randomizer;
    public float randomScale;
    public float maximumRandom;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(WeaponSpread());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public IEnumerator WeaponSpread()
    {
        //Endless loop to handle weapon spread, less cpu intensive than starting a new routine every time
        while (true)
        {
            //If the player is shooting and spread hasn't reached maximum spread increase spread
            if (shooting && randomScale < maximumRandom)
                randomScale += .1f;
            //Otherwise if the random scale is still positive and the player is not shooting
            else if (randomScale > 0 && !shooting)
                randomScale -= .1f;
            //Wait 1/10 of a second
            yield return new WaitForSeconds(.1f);
        }
    }

    public override int Shoot()
    {
        if (Time.time < TimeToFire)
            return 1;
        Debug.Log(TimeToFire);
        Vector2 playerPos = GameMaster.playerObject.transform.position;
        //First see if the player can fire the weapon
        int test = base.Shoot();
        if (test != 0)
            return 1;
        //Make a new randomizer for spread
        randomizer = new Vector2(Random.Range(-randomScale, randomScale), Random.Range(-randomScale, randomScale));
        //Store hit
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, playerPos - firePointPosition + randomizer, 100, whatToHit);
        //If something was hit
        if (hit.collider != null)
        {
            //If hit was an enemy, damage enemy
            Player player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer(damage);
            }
        }
        //If it's time to spawn a bullet trail
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            //If the bullet hit nothing don't calculate what they hit and just extend the line very far
            if (hit.collider == null)
            {
                hitPos = (playerPos - firePointPosition + randomizer) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            //Otherwise make the line in between the fire point and what the player hit
            else
            {
                hitPos = hit.point + randomizer;
                hitNormal = hit.normal;
            }
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        return 0;
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        //Instantiate the trail so it can be modified
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null)
        {
            //Draw line
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        else
        {
            Debug.LogError("No line renderer found for a Hitscan weapon");
        }

        //Destroy it after .05 seconds
        Destroy(trail.gameObject, 0.05f);

        //Make a particle if the player hit something
        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        //Create a muzzleflash
        Transform muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint) as Transform;
        //Randomize size
        float size = Random.Range(0.6f, 0.9f);
        muzzleFlash.localScale = new Vector3(size, size, 0);
        Destroy(muzzleFlash.gameObject, 0.02f);
    }
}
