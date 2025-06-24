using System.Collections.Generic;
using UnityEngine;
public enum EBgm
{
    //BGM_TITLE,
    BGM_GAME
}
public enum ESfx
{
    Hit,
    Item,
    Jump,
    Walk,
    Death,
    BombVisible,
    BombExplode,
    StageOver,
    StageClear,
    PowerUp,
    PowerDown,
    Respawn
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Clips")] //audio 플레이하는 장치
    [SerializeField] private AudioClip[] bgmClips;  // BGM 클립 배열
    [SerializeField] private AudioClip[] sfxClips; // SFX 클립 배열

    [Header("Audio Sources")] //audio 소스
    [SerializeField] private AudioSource bgmSource; // BGM 재생 AudioSource
    [SerializeField] private AudioSource sfxSource; // SFX 재생 AudioSource

    private Dictionary<EBgm, AudioClip> bgmDict; // BGM Dictionary
    private Dictionary<ESfx, AudioClip> sfxDict; // SFX Dictionary

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitDictionaries();
    }
    private void InitDictionaries()
    {
        bgmDict = new Dictionary<EBgm, AudioClip>();
        foreach (EBgm bgm in System.Enum.GetValues(typeof(EBgm)))
        {
            var clip = Resources.Load<AudioClip>($"Audio/BGM/{bgm}");
            if (clip != null)
                bgmDict[bgm] = clip;
            else
                Debug.LogWarning($"BGM clip not found: {bgm}");
        }

        sfxDict = new Dictionary<ESfx, AudioClip>();
        foreach (ESfx sfx in System.Enum.GetValues(typeof(ESfx)))
        {
            var clip = Resources.Load<AudioClip>($"Audio/SFX/{sfx}");
            if (clip != null)
                sfxDict[sfx] = clip;
            else
                Debug.LogWarning($"SFX clip not found: {sfx}");
        }
    }
    public void PlayBGM(EBgm bgmType)
    {
        if (bgmDict.TryGetValue(bgmType, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else Debug.LogWarning("BGM not found in Dictionary!");
        
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(ESfx sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else Debug.LogWarning("SFX not found in Dictionary!");
    }
}