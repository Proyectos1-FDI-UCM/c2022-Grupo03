using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Dropdown resolutionDropdown;
    [SerializeField]
    private GameObject controls;   
    private GameObject MKcontrols; 
    private GameObject JScontrols;

    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> resOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height) 
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        MKcontrols = controls.transform.GetChild(2).transform.gameObject;
        JScontrols = controls.transform.GetChild(3).transform.gameObject;
    }
    public void volumeControl (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void ShowControls()
    {
        if (!controls.activeSelf) controls.SetActive(true);
        else controls.SetActive(false);
    }    
    public void ShowControlType()
    {
        if (!MKcontrols.activeSelf)
        {
            MKcontrols.SetActive(true);
            JScontrols.SetActive(false);
        }
        else
        {
            MKcontrols.SetActive(false);
            JScontrols.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
