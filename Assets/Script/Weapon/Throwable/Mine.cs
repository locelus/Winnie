using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Throwable {

    public ParticleSystem particleExplosion;
    public GameObject mine;

    void OnTriggerEnter2D (Collider2D _colInfo) {
        //debugging purposes
        Debug.LogWarning("Mine exploded");
        Explode();
        //Mine no longer needed, so destroy
        Destroy(mine);
    }
	
	void Explode () {
        //Instantiate the explosion
        Instantiate(particleExplosion, this.transform.position, Quaternion.identity);
	}
}
