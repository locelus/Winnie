using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Keybindings : MonoBehaviour {

    [Header("Pause Menus")]
    public KeyCode pauseMenuKeyCode;
    public GameObject[] pauseMenus;
    private int pauseMenuIndex = 0;


    public void Update() {
        //TODO: Add animations for settings menu
        if (Input.GetKeyDown(pauseMenuKeyCode)) { 
            PauseMenu();
        }
    }

    public void PauseMenu() {
        if (pauseMenus[0].activeInHierarchy == true) {
            //Close settings menu and resume the game
            pauseMenus[0].SetActive(false);
            Pause(false);
            return;
        } else {
            for (int i = 0; i < pauseMenus.Length; i++) {
                if (pauseMenus[i].activeInHierarchy == false) {
                    pauseMenuIndex++;
                    if (pauseMenuIndex == pauseMenus.Length) {
                        //If none of the secondary windows are active open the main settings menu (pauseMenus[0])
                        pauseMenus[0].SetActive(true);
                        pauseMenuIndex = 0;
                        Pause(true);
                        return;
                    }
                }
                else if (pauseMenus[i].activeInHierarchy == true) {
                    //If there is a secondary window open, then return to the main window
                    pauseMenus[i].SetActive(false);
                    pauseMenus[0].SetActive(true);
                    return;
                }
            }
        }
    }


    public void Pause(bool pause) {
        if (pause) {
            Time.timeScale = 0;
        }
        if(!pause) {
            Time.timeScale = 1;
        }
        AudioListener.pause = pause;
    }
}