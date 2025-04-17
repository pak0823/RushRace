using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class AudioMixerVolumeControl : MonoBehaviour
{
    [Header("�ͼ� �� �Ķ���� �̸�")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGMVolume";
    public string sfxParameter = "SFXVolume";

    private Slider bgmSlider;
    private Slider sfxSlider;

    // OptionWindow���� �����̴��� ������ �� ȣ���
    public void InitializeSliders(Slider bgm, Slider sfx)
    {
        bgmSlider = bgm;
        sfxSlider = sfx;

        // PlayerPrefs���� �ʱ� ���� �� �ε�
        float bgmValue = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;

        ApplyVolume(bgmParameter, bgmValue);
        ApplyVolume(sfxParameter, sfxValue);

        // �����̴� �̺�Ʈ ����
        bgmSlider.onValueChanged.AddListener((value) => {
            ApplyVolume(bgmParameter, value);
            PlayerPrefs.SetFloat("BGMVolume", value);
        });

        sfxSlider.onValueChanged.AddListener((value) => {
            ApplyVolume(sfxParameter, value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        });
    }

    // AudioMixer ���� ���� �Լ�
    private void ApplyVolume(string parameterName, float sliderValue)
    {
        float volumeInDecibel = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        volumeInDecibel = Mathf.Clamp(volumeInDecibel, -80f, 0f); // �ִ� ���� ����
        audioMixer.SetFloat(parameterName, volumeInDecibel);
    }
}
