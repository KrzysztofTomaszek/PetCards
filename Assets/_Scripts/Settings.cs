using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown graphic;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject proces;
    [SerializeField] Slider mVolume;
    [SerializeField] Slider bVolume;
    [SerializeField] Slider sVolume;
    Resolution[] resolutions;

    void Start()
    {
        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].height >= 600)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);
            }
            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        string currentResoluton = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
        dropdown.RefreshShownValue();
        int index = dropdown.options.FindIndex((i) => { return i.text.Equals(currentResoluton); });
        dropdown.value = index;
        graphic.value = QualitySettings.GetQualityLevel();
        toggle.isOn = Screen.fullScreen;
        float volume;
        audioMixer.GetFloat("overallVolume", out volume);
        mVolume.value = volume;
        audioMixer.GetFloat("bgmusicVolume", out volume);
        bVolume.value = volume;
        audioMixer.GetFloat("soundVolume", out volume);
        sVolume.value = volume;
    }
    public void resolutionChange(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void overallAudioChange(float volume)
    {
        audioMixer.SetFloat("overallVolume", volume);
    }
    public void bgmusicChange(float volume)
    {
        audioMixer.SetFloat("bgmusicVolume", volume);
    }
    public void soundsChange(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
    }
    public void graphicChange(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
