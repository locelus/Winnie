using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {

    private static int weaponIndex = 0;
    public GameObject[] weapons;
    public KeyCode[] hotkeys;
    private static int scrollWheelIndex;

    private void Start() {
        ChangeWeapon(weaponIndex);
    }


    // Update is called once per frame
    void Update () {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {  //If player scrolls down
            scrollWheelIndex++;                         //Add to scrollWheelIndex for roll around purposes
            if (scrollWheelIndex + 1 > weapons.Length) {
                scrollWheelIndex = 0;
            }
            ChangeWeapon(scrollWheelIndex);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {  //If player scrolls up
            if (scrollWheelIndex == 0) {
                scrollWheelIndex = weapons.Length - 1;
                ChangeWeapon(scrollWheelIndex);
            }
            else {
                scrollWheelIndex--;
                ChangeWeapon(scrollWheelIndex);
            }
        }
        for (int i = 0; i < hotkeys.Length; i++) {
            if (Input.GetKeyDown(hotkeys[i])) {
                ChangeWeapon(i);
            }
        }
    }

    void ChangeWeapon(int i) {
        if (weapons[i].activeSelf != true) {
            for (int n = 0; n < weapons.Length; n++) {
                weapons[n].SetActive(false);
            }
            weapons[i].SetActive(true);
            weaponIndex = i;
        }
    }
}
