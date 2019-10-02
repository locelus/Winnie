using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [HideInInspector]
    public Camera mainCam;

    public float shakeAmount = 0;

    // Use this for initialization
    void Awake() {
        if (mainCam == null) {
            mainCam = Camera.main; //If there is no camera, set the value
        }
    }

    public void Shake(float amt, float length) {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f); 
        Invoke("StopShake", length);
    }

    private void DoShake() {
        if (shakeAmount > 0) {
            Vector3 camPos = mainCam.transform.position;
            //Set the camera's x and y positions to random values and then change the camera's position back
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }

    private void StopShake() {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
