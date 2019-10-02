using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Pickup {
    
    void OnTriggerEnter2D(Collider2D colInfo) {
        //If something other than a player triggered this, return
        if (!IsPlayer(colInfo))
            return;
        //Get the player component
        Player player = colInfo.GetComponent<Player>();
        //If the player is not already max health
        if (player.stats.curHealth != player.stats.maxHealth) {
            //Negatively damage the player, healing him
            player.DamagePlayer(-changer);
            Destroy(this.gameObject);
        }
    }

}