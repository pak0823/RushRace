using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerVolumeControl : MonoBehaviour
{
    [Header("�ͼ� & �Ű����� �̸�")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGMVolume";
    public string sfxParameter = "SFXVolume";

    [Header("�����̴���")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // �����̴� �ʱ�ȭ
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        ApplyVolume(bgmParameter, bgm);
        ApplyVolume(sfxParameter, sfx);

        // �̺�Ʈ ���
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    void OnBGMVolumeChanged(float value)
    {
        ApplyVolume(bgmParameter, value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        ApplyVolume(sfxParameter, value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void ApplyVolume(string parameterName, float sliderValue)
    {
        // AudioMixer�� ���ú� ���� ��� �� �α� ��ȯ �ʿ�
        float volumeInDecibel = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(parameterName, volumeInDecibel);
    }
}
