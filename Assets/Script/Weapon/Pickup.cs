using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    
    public float changer;
    //Checks whether or not the collider passed has a player tag
    public bool IsPlayer(Collider2D col) {
        if (col.tag == "Player")
            return true;
        else
            return false;
    }
}
