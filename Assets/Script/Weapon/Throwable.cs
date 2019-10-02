using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Weapon {

    KeyCode grenadeCode;
    public GameObject throwable;
    Projectile projectile;

    // Use this for initialization
    public override void Awake () {
        base.Awake();
        projectile = throwable.GetComponent<Projectile>();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        //Instantiate the grenade when initially pressed
        if (Input.GetKeyDown(grenadeCode)) {
            Instantiate(throwable);
        }
        //When the key is lifted up, throw the grenade
        if (Input.GetKeyUp(grenadeCode)) {
            projectile.Throw();
        }
    }
}
