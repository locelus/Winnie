using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    //Amount of times the weapon can be used per second
    public float fireRate;
    //Amount of damage per hit (direct hit if explosive)
    public float damage;
    //What the weapon can hit
    public LayerMask whatToHit;
    //Whether or not the weapon is semi auto
    public bool semiAuto;
    //Whether or not the player is currently using the weapon
    [HideInInspector]
    public bool shooting;
    //A public version of the TimeToFire variable
    public float TimeToFire {
        get { return timeToFire; }
        set { timeToFire = value; }
    }
    //The real version
    private float timeToFire = 0;
    //Where the weapon shoots from, or where the hit is for melee weapons
    [HideInInspector]
    public Transform firePoint;
    //Stores the current mouse position
    [HideInInspector]
    public Vector2 mousePosition;
    //Stores the firePointPosition
    [HideInInspector]
    public Vector2 firePointPosition;

    public bool shootable = true;

    //When the weapon is enabled (when the player switches to it), update the ammo text to match the current weapon
    private void OnEnable() {
        //Keep this here, idk if we'll need it
    }
  
	// Use this for initialization
	public virtual void Awake () {
        //Stores the firepoint
		firePoint = transform.Find("Firepoint");
        //If there is no firepoint, throw an error
        if (firePoint == null) {
            Debug.LogError("Firepoint is null");
        }
    }
 
    public virtual void Update () {
        //If the player stops pressing M1 or the current ammo is 0, then stop shooting
        if (Input.GetButtonUp("Fire1"))
            shooting = false;
        if (semiAuto) {
            //If the gun is semi auto, only call the shoot function when the mouse button is actually pressed
            if (Input.GetButtonDown("Fire1") && Time.time > TimeToFire) {
                TimeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        } else {
            if (Input.GetButton("Fire1") && Time.time > TimeToFire) {
                TimeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
        //Keep the TimeToFire variable consistent
    }
    
    //Use the weapon, if there is some reason a weapon can't be used the function will return a number other than 0
    public virtual int Shoot() {
        //If the game is paused, don't let the shoot function continues
        if (Time.timeScale == 0)
            return 1;
        shooting = true;
        //Get the mouse position and fire point position for processing
        mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        return 0;
	}

}                               
                                