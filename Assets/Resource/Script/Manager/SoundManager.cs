// SoundManager.cs - 전반적인 BGM/SFX 재생 및 볼륨 관리

using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource sfxCarLoopSource;

    public AudioMixer audioMixer;

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

    public void SetBGMVolume(float value)
    {
        ApplyMixerVolume("BGMVolume", value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        ApplyMixerVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void PlaySound(SoundData data)
    {
        if (data == null || data.clip == null) return;

        switch (data.type)
        {
            case eSOUNDTYPE.eSOUND_BGM:
                bgmSource.clip = data.clip;
                bgmSource.volume = data.volume * 0.5f;
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

            case eSOUNDTYPE.eSOUND_COIN:
                if (sfxSource.clip != data.clip)
                    sfxSource.PlayOneShot(data.clip, data.volume * 3f);
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
        // 일시정지 상태면 바로 리턴
        if (Time.timeScale == 0f)
            return;

        // 미션이 끝났거나 활성화되지 않은 상태면 리턴
        if (Shared.MissionManager != null && !Shared.MissionManager.IsMissionActive)
            return;

        if (data == null || data.clip == null)
            return;

        if (sfxCarLoopSource.clip != data.clip)
        {
            sfxCarLoopSource.clip = data.clip;
            sfxCarLoopSource.volume = data.volume * 0.2f;
            sfxCarLoopSource.loop = true;
            sfxCarLoopSource.Play();
        }
    }

    public void ResumeLoopSound()
    {
        if (sfxCarLoopSource != null && sfxCarLoopSource.clip != null && !sfxCarLoopSource.isPlaying)
        {
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
