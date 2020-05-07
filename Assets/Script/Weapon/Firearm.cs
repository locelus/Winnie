using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Firearm : Weapon
{
    //The maximum amount of ammo this weapon can have in the mag
    public float maxAmmo;
    //Amount of time it takes the weapon to reload
    public float reloadTime;
    //Whether or not this weapon is reloadable
    public bool reloadable;
    //Stores whether or not the player is currently reloading
    public bool reloading = false;
    //Is set in Unity, the amount of ammo is displayed here
    Text ammoText;

    //Whether or not the gun needs ammo to fire
    public bool needsAmmo;

    //The current amount of ammunition the player has
    [HideInInspector]
    public float curAmmo;
    
    // Start is called before the first frame update
    public override void Awake()
    {
        //Stores the ammo text component
        ammoText = GameObject.Find("Ammo Text").GetComponent<Text>();
        //Sets the current ammo to the max ammo
        curAmmo = maxAmmo;
        //Update the ammo text
        UpdateAmmoText();
        base.Awake();
    }

    //The function that updates the ammo text
    public void UpdateAmmoText() {
        //If the current ammo is negative (this should only happen if the weapon has infinite ammunition), then 
        //say the ammo is infinite
        if (curAmmo < 0) {
            ammoText.text = "Ammo: Infinite";
            return;
        }
        //If the current ammo is not negative, then we can actually set it
        ammoText.text = "Ammo: " + curAmmo.ToString() + "/" + maxAmmo.ToString();
    }

    void OnEnable() {
        UpdateAmmoText();
    }

    // Update is called once per frame
    public override void Update()
    {
        //If there's no more ammo and the gun needs ammo
        if (curAmmo <= 0 && needsAmmo) {
            shooting = false;
        }
        base.Update();
        //If we need to start reloading then reload
        if (Input.GetButtonDown("Reload") && reloadable) {
            StartCoroutine(Reload());
        }
    }
    
    public void AddAmmo(float changer) {
        //Add to current ammo the amount
        curAmmo =+ changer;
        //Make sure ammo is within range
        curAmmo = Mathf.Clamp(curAmmo, 0f, maxAmmo);
        UpdateAmmoText();
    }
    
    public IEnumerator Reload() {
        Debug.Log("Reload function called");
        //If gun already has maximum ammo
        if (curAmmo == maxAmmo) {
            StopCoroutine(Reload());
        }
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        AddAmmo(maxAmmo);
        reloading = false;
    }

    public override int Shoot() {
        if ((curAmmo <= 0 && needsAmmo) || reloading)
            return 1;
        int test = base.Shoot();
        if (test == 1)
        {
            return 1;
        }
        //If the current ammo is existent
        if (curAmmo > 0 && needsAmmo) {
            curAmmo--;
            UpdateAmmoText();
        }
        return 0;
    }
}
