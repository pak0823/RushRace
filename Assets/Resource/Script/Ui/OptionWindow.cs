using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [Header("볼륨 슬라이더")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("볼륨 컨트롤 스크립트")]
    public AudioMixerVolumeControl volumeControl;


    private void Start()
    {
        // 슬라이더 초기화 및 AudioMixerVolumeControl에 전달
        if (volumeControl != null)
        {
            volumeControl.InitializeSliders(bgmSlider, sfxSlider);
        }

        gameObject.SetActive(false);
    }
}
