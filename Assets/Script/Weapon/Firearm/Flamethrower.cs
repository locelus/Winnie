using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Firearm {
    
    public ParticleSystem flameParticle;
    public Transform firepoint;
    

    public override void Awake() {
        base.Awake();
        var main = flameParticle.main;
        main.customSimulationSpace = GameMaster.gm.transform;       //Bug where the prefab of the player didn't have the gm set as
    }                                                               //local transform, this should fix it

    public override int Shoot() {
        base.Shoot();
        //Getting the flamethrower's particle system
        var emission = flameParticle.emission;
        //If there's no ammo, don't fire
        if (curAmmo <= 0) {
            emission.enabled = false;
            return 1;
        }
        //Enable emission if we made it this far
        emission.enabled = true;
        return 0;
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        var emission = flameParticle.emission;
        //Stop firing if the player stops holding Fire1
        if (Input.GetButtonUp("Fire1")) {
            emission.enabled = false;
        }
	}
}
