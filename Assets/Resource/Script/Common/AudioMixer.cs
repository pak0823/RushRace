using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerVolumeControl : MonoBehaviour
{
    [Header("믹서 & 매개변수 이름")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGMVolume";
    public string sfxParameter = "SFXVolume";

    [Header("슬라이더들")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // 슬라이더 초기화
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        ApplyVolume(bgmParameter, bgm);
        ApplyVolume(sfxParameter, sfx);

        // 이벤트 등록
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
        // AudioMixer는 데시벨 단위 사용 → 로그 변환 필요
        float volumeInDecibel = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(parameterName, volumeInDecibel);
    }
}
