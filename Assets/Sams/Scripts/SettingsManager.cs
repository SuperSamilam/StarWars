using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{   
    [SerializeField] AudioSource source;
    [SerializeField] FontIconSelector fontIconSelector;
    [SerializeField] UnityEngine.UI.Slider slider;

    void Start()
    {
        ChangeIcon();
        ChangeAudio(0);
    }

    public void VoiceButtonClicked()
    {
        if (PlayerPrefs.GetInt("Voice") == 0)
        {
            PlayerPrefs.SetInt("Voice", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Voice", 0);
        }
        ChangeIcon();
    }

    //Changes the use voiceline icon
    void ChangeIcon()
    {
        if (PlayerPrefs.GetInt("Voice") == 0)
        {
            fontIconSelector.CurrentIconName = "Icon 23";
        }
        else
        {
            fontIconSelector.CurrentIconName = "Icon 22";
        }
    }
    
    //Changes the audio and updates the icons for it
    public void ChangeAudio(float amount)
    {
        PlayerPrefs.SetFloat("Audio", PlayerPrefs.GetFloat("Audio") + amount);
        slider.value = PlayerPrefs.GetFloat("Audio");
        source.volume = PlayerPrefs.GetFloat("Audio");
    }
}
