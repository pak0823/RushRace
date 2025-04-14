// OptionWindow.cs - 옵션창에서 BGM/SFX 볼륨 제어

using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        Shared.SoundManager.SetBGMVolume(bgm);
        Shared.SoundManager.SetSFXVolume(sfx);
    }

    public void OnBGMVolumeChanged(float value)
    {
        Shared.SoundManager.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        Shared.SoundManager.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
