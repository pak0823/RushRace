using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [Header("���� �����̴�")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("���� ��Ʈ�� ��ũ��Ʈ")]
    public AudioMixerVolumeControl volumeControl;


    private void Start()
    {
        // �����̴� �ʱ�ȭ �� AudioMixerVolumeControl�� ����
        if (volumeControl != null)
        {
            volumeControl.InitializeSliders(bgmSlider, sfxSlider);
        }

        gameObject.SetActive(false);
    }
}
