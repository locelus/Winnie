using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer mainVolume;

    public Dropdown resolutionDropdown;

    public Dropdown qualityDropdown;

    public Slider volumeSlider;
    float currentVolume;
    bool volumeResult;
    
    Resolution[] resolutions;

    public Dropdown fullscreenDropdown;

    private void Start() {
        if (PlayerPrefs.HasKey("Main Volume")) {
            mainVolume.SetFloat("MainVolume", PlayerPrefs.GetFloat("Main Volume"));
        } else {
            mainVolume.SetFloat("MainVolume", 0f);
        }
        /*if (!PlayerPrefs.HasKey("Fullscreen Mode")) {
            PlayerPrefs.SetInt("Fullscreen Mode", 1);
        }
        int fullScreenMode = PlayerPrefs.GetInt("Fullscreen Mode");
        Screen.fullScreenMode = (FullScreenMode)fullScreenMode;
        */

        //Setting up our resolutions list
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height + " (" + resolutions[i].refreshRate + ")";
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        

        resolutionDropdown.value = currentResolutionIndex;
    }

    public void UpdateValues() {
        qualityDropdown.value = QualitySettings.GetQualityLevel(); //Quality refresh
        volumeResult = mainVolume.GetFloat("MainVolume", out currentVolume); //Volume refresh
        volumeSlider.value = currentVolume;
        /*Debug.Log(Screen.fullScreenMode);
        fullscreenDropdown.value = (int)Screen.fullScreenMode;
        Debug.Log((int)Screen.fullScreenMode);*/
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex]; 
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetVolume(float volume) {
        mainVolume.SetFloat("MainVolume", volume);
        PlayerPrefs.SetFloat("Main Volume", volume);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(int fullScreen) {
        Screen.fullScreenMode = (FullScreenMode)fullScreen;
    }
	
}
