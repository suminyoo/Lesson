using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Sound Clips")]
    public List<Sound> soundList;

    private Dictionary<string, AudioClip> soundDict;

    void Awake()
    {
        // ½Ì±ÛÅæ ÆÐÅÏ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ¸®½ºÆ® -> µñ¼Å³Ê¸®·Î º¯È¯
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in soundList)
        {
            soundDict[sound.name] = sound.clip;
        }
    }

    // È¿°úÀ½ Àç»ý
    public void PlaySFX(string name)
    {
        if (soundDict.ContainsKey(name))
        {
            sfxSource.PlayOneShot(soundDict[name]);
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found!");
        }
    }

    // ¹è°æÀ½¾Ç Àç»ý
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}