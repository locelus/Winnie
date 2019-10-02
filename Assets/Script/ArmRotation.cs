using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 90;
    private float _rotZ;

    public float rotZ {
        get { return _rotZ; }
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale == 0) {
            return;
        }// subtracting the position of the player from the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();     // normalizing the vector. Meaning that all the sum of the vector will be equal to 1
                                           
        float _rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;   // find the angle in degrees
        
        transform.rotation = Quaternion.Euler(0f, 0f, _rotZ + rotationOffset);
    }
}