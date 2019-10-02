using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargable : ProjectileWeapon
{
    //Measures the strength of the projectile being fired
    [HideInInspector]
    public float pullStrength = 0;
    //Amount of seconds it takes the projectile to charge to full strength
    public float secondsToFullStrength = 1;
    //The maximum pull strength (multiplicative in relation to projectile)
    public float maxPullStrength = 1;
    //Whether or not the weapon is currently being charged
    bool charging = false;
    //Time until the player can begin charging again
    float timeTillNextShot = 0;
    
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        //When Fire1 is initially pressed, if charging is false then set charging to true
        if (Input.GetButtonDown("Fire1") && charging == false) {
            charging = true;
        }
        //If Fire1 is being held down
        if (Input.GetButton("Fire1")) {
            //If maxPullStrength has not been reached, and it is possible to begin charging
            if (pullStrength <= maxPullStrength && Time.time > timeTillNextShot) {
                //Add to pullStrength, on the linear scale
                pullStrength += maxPullStrength * (Time.deltaTime / secondsToFullStrength);
            }
        }
        //If Fire1 is lifted and the weapon is currently charging (ie it's possible to fire)
        if (Input.GetButtonUp("Fire1") && charging) {
            charging = false;
            Shoot();
            pullStrength = 0;
        }
        base.Update();
    }

    public override int Shoot() {
        //If the weapon is still charging or it's not possible to fire yet, stop the function
        if (charging || Time.time < timeTillNextShot) {
            return 1;
        }
        timeTillNextShot = Time.time + (1/fireRate);
        base.Shoot();
        //Get the Projectile script from the projectile spawned by ProjectileWeapon
        projectileScript = projectile.GetComponent<Projectile>();
        //Change the projectile's speed so it is relative to the amount of time spent charging
        projectileScript.movementForce = projectileScript.movementForce * pullStrength;
        return 0;
    }
    
}
