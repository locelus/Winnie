using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAI : MonoBehaviour {

    public bool seen;
    public EnemyWeapon weaponScript;
    public GameObject weapon;
    float maxDistance = 10f;
    RaycastHit2D hit;
    Vector3 rayDirection;
    public LayerMask mask;
    public GameObject sightPoint;

    public void Start() {
        weaponScript = weapon.GetComponent<EnemyWeapon>();
        seen = false;
    }

    public void Update() {
        rayDirection = GameMaster.playerObject.transform.position - sightPoint.transform.position;
        hit = Physics2D.Raycast(sightPoint.transform.position, rayDirection, mask);
        if (hit.transform == GameMaster.playerObject.transform) {   //If the object hit by the raycast is the player...
            seen = true;
        }       //If player is visible, then shoot
        else {
            seen = false;
        }
        if (seen) {
            weaponScript.Shoot();
        }
        Debug.DrawRay(sightPoint.transform.position, rayDirection);
    }


}
