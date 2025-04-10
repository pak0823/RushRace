// SoundManager.cs - AudioMixer 방식으로 수정

using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource sfxCarLoopSource;

    public AudioMixer audioMixer; // AudioMixer 참조

    private void Awake()
    {
        if (Shared.SoundManager == null)
        {
            Shared.SoundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource는 Inspector에서 Output을 BGM/SFX 그룹으로 연결할 것
        bgmSource = transform.Find("BGMSource").GetComponent<AudioSource>();
        sfxSource = transform.Find("SFXSource").GetComponent<AudioSource>();

        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ApplyMixerVolume("BGMVolume", bgm);
        ApplyMixerVolume("SFXVolume", sfx);
    }

    void ApplyMixerVolume(string parameter, float sliderValue)
    {
        float volumeInDecibel = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(parameter, volumeInDecibel);
    }

    public void PlaySound(SoundData data)
    {
        if (data == null || data.clip == null) return;

        switch (data.type)
        {
            case eSOUNDTYPE.eSOUND_BGM:
                bgmSource.clip = data.clip;
                // volume은 AudioMixer에서 제어
                bgmSource.loop = true;
                bgmSource.Play();
                break;
            case eSOUNDTYPE.eSOUND_UI:
                sfxSource.PlayOneShot(data.clip);
                break;
            case eSOUNDTYPE.eSOUND_CAR:
                if (sfxSource.clip != data.clip)
                    sfxSource.PlayOneShot(data.clip);
                break;
        }
    }

    public void StopPlaySound()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void PlayLoopSound(SoundData data)
    {
        if (data == null || data.clip == null)
            return;

        if (sfxCarLoopSource.clip != data.clip)
        {
            sfxCarLoopSource.clip = data.clip;
            sfxCarLoopSource.loop = true;
            sfxCarLoopSource.Play();
        }
    }

    public void StopLoopSound()
    {
        if (sfxCarLoopSource.isPlaying)
            sfxCarLoopSource.Stop();
        sfxCarLoopSource.clip = null;
    }
}
