using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Pickup {
    
    void OnTriggerEnter2D(Collider2D colInfo) {
        //If something other than a player triggered this, return
        if (!IsPlayer(colInfo))
            return;
        Firearm firearm = colInfo.GetComponentInChildren<Firearm>();
        if (firearm == null) {
            //If there was no firearm, return
            Debug.LogError("WEAPON WAS NULL");
            return;
        }
        if (firearm.maxAmmo != firearm.curAmmo) {
            //Add ammo
            firearm.AddAmmo(changer);
            Destroy(this.gameObject);
        }
    }

}