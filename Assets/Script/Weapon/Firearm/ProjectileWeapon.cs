using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Firearm {

    //Stores effect to be played on impact
    public GameObject impactEffect;
    //Reference to the prefab of the projectile for this weapon
    public GameObject projectilePrefab;
    //Stores the actual weapon once spawned
    [HideInInspector]
    public GameObject projectile;
    //Stores the projectile's script
    [HideInInspector]
    public Projectile projectileScript;
    //Reference to player's arm
    GameObject arm;


	public override void Awake () { 
        base.Awake();
        arm = transform.parent.gameObject;
    }
	
	public override void Update () {
        base.Update();
    }
    
    public override int Shoot() {
        //If it's not possible to shoot, stop the function
        if (base.Shoot() == 1)
            return 1;
        //Fire projectile and set it to projectile
        projectile = Instantiate(projectilePrefab, firePoint.position, arm.transform.rotation);
        return 0;
    }

}
