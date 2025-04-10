using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class AudioMixerVolumeControl : MonoBehaviour
{
    [Header("믹서 및 파라미터 이름")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGMVolume";
    public string sfxParameter = "SFXVolume";

    private Slider bgmSlider;
    private Slider sfxSlider;

    // OptionWindow에서 슬라이더를 연결할 때 호출됨
    public void InitializeSliders(Slider bgm, Slider sfx)
    {
        bgmSlider = bgm;
        sfxSlider = sfx;

        // PlayerPrefs에서 초기 볼륨 값 로드
        float bgmValue = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;

        ApplyVolume(bgmParameter, bgmValue);
        ApplyVolume(sfxParameter, sfxValue);

        // 슬라이더 이벤트 연결
        bgmSlider.onValueChanged.AddListener((value) => {
            ApplyVolume(bgmParameter, value);
            PlayerPrefs.SetFloat("BGMVolume", value);
        });

        sfxSlider.onValueChanged.AddListener((value) => {
            ApplyVolume(sfxParameter, value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        });
    }

    // AudioMixer 볼륨 적용 함수
    private void ApplyVolume(string parameterName, float sliderValue)
    {
        float volumeInDecibel = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        volumeInDecibel = Mathf.Clamp(volumeInDecibel, -80f, 0f); // 최대 음량 제한
        audioMixer.SetFloat(parameterName, volumeInDecibel);
    }
}
