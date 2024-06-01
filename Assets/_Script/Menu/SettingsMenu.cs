using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public Dropdown resolutionDropDown;

    public void Start()
    {
        resolutions = Screen.resolutions.Select(Resolution => new Resolution { width = Resolution.width, height = Resolution.height }).Distinct().ToArray();
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int IndexBestRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                IndexBestRes = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = IndexBestRes;
        resolutionDropDown.RefreshShownValue();

        Screen.fullScreen = true;
    }

    public void setVolume(float value)
    {
        audioMixer.SetFloat("volume", value);
    }

    public void setSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", value);
    }

    public void setMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVolume", value);
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    
}
