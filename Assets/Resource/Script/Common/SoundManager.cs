using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    //public AudioSource sfxUISource;
    //public AudioSource sfxCarSource;
    public AudioSource sfxCarLoopSource;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

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
        }

        bgmSource = transform.Find("BGMSource").GetComponent<AudioSource>();
        sfxSource = transform.Find("SFXSource").GetComponent<AudioSource>();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
        if (sfxCarLoopSource != null)
            sfxCarLoopSource.volume = sfxVolume;
    }

    public void PlaySound(SoundData data)
    {
        switch (data.type)
        {
            case eSOUNDTYPE.eSOUND_BGM:
                bgmSource.clip = data.clip;
                bgmSource.volume = data.volume;
                bgmSource.loop = true;
                bgmSource.Play();
                break;
            case eSOUNDTYPE.eSOUND_UI:
                sfxSource.PlayOneShot(data.clip, data.volume);
                break;
            case eSOUNDTYPE.eSOUND_CAR:
                if (sfxSource.clip != data.clip)
                    sfxSource.PlayOneShot(data.clip, data.volume);
                break;
                //case eSOUNDTYPE.eSOUND_SHOP:
                //    sfxShopSource.PlayOneShot(data.clip, data.volume);
                //    break;
                //case eSOUNDTYPE.eSOUND_REPAIR:
                //    sfxRepairSource.PlayOneShot(data.clip, data.volume);
                //    break;
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
            sfxCarLoopSource.volume = data.volume;
            sfxCarLoopSource.loop = true;
            sfxCarLoopSource.Play();
        }
        // 같은 사운드면 아무것도 하지 않음
    }

    public void StopLoopSound()
    {
        if (sfxCarLoopSource.isPlaying)
            sfxCarLoopSource.Stop();
        sfxCarLoopSource.clip = null;
    }
}