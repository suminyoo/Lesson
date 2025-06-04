using System.Collections.Generic;
using UnityEngine;

public enum AudioType
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
    PowerUP,
    PowerDown

}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Dictionary<AudioType, AudioClip> playList;

    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip item;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip bombVisible;
    [SerializeField] AudioClip bombExplode;
    [SerializeField] AudioClip stageOver;
    [SerializeField] AudioClip stageClear;
    [SerializeField] AudioClip powerUP;
    [SerializeField] AudioClip powerDown;


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
    void Start()
    {
        playList = new Dictionary<AudioType, AudioClip>();
        playList.Add(AudioType.Hit, hit);
        playList.Add(AudioType.Item, item);
        playList.Add(AudioType.Jump, jump);
        playList.Add(AudioType.Walk, walk);
        playList.Add(AudioType.Death, death);
        playList.Add(AudioType.BombVisible, bombVisible);
        playList.Add(AudioType.BombExplode, bombExplode);
        playList.Add(AudioType.StageOver, stageOver);
        playList.Add(AudioType.StageClear, stageClear);
        playList.Add(AudioType.PowerUP, powerUP);
        playList.Add(AudioType.PowerDown, powerDown);

    }

    public void PlayOneList(AudioType myType)
    {
        AudioClip clip = playList[myType];
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Debug.Log("play Audio"+ clip);
    }

}
//using System.Collections.Generic;
//using UnityEngine;



//public class SoundManager : MonoBehaviour
//{
//    public static SoundManager Instance;

//    [Header("Audio Clips")] //audio 플레이하는 장치
//    [SerializeField] private AudioClip[] bgmClips;  // BGM 클립 배열
//    [SerializeField] private AudioClip[] sfxClips; // SFX 클립 배열

//    [Header("Audio Sources")] //audio 소스
//    [SerializeField] private AudioSource bgmSource; // BGM 재생 AudioSource
//    [SerializeField] private AudioSource sfxSource; // SFX 재생 AudioSource

//    private Dictionary<EBgm, AudioClip> bgmDict; // BGM Dictionary
//    private Dictionary<ESfx, AudioClip> sfxDict; // SFX Dictionary
//    public enum EBgm
//    {
//        TITLE,
//        INGAME,
//        ENDING,
//    }
//    public enum ESfx
//    {
//        HIT,
//        STAGEOVER,
//        STAGECLEAR,
//        ITEM,
//        JUMP,
//        RUN
//    }

//    private void InitDictionaries()
//    {
//        bgmDict = new Dictionary<EBgm, AudioClip>();
//        for (int i = 0; i < bgmClips.Length; i++)
//        {
//            bgmDict[(EBgm)i] = bgmClips[i];
//        }
//        sfxDict = new Dictionary<ESfx, AudioClip>();
//        for (int i = 0; i < sfxClips.Length; i++)
//        {
//            sfxDict[(ESfx)i] = sfxClips[i];
//        }
//    }

//    public void PlayBGM(EBgm bgmType)
//    {
//        if (bgmDict.TryGetValue(bgmType, out var clip))
//        {
//            bgmSource.clip = clip;
//            bgmSource.loop = true; // 배경음악은 기본적으로 반복 재생
//            bgmSource.Play();
//            Debug.LogWarning("BGM playing" + bgmType + " " + clip);

//        }
//        else
//        {
//            Debug.LogWarning("BGM not found in Dictionary!");
//        }
//    }
//    public void StopBGM()
//    {
//        bgmSource.Stop();
//    }

//    public void PlaySFX(ESfx sfxType)
//    {
//        if (sfxDict.TryGetValue(sfxType, out var clip))
//        {
//            sfxSource.PlayOneShot(clip);
//            Debug.LogWarning("SFX playing" + sfxType + " " + clip);
//        }
//        else
//        {
//            Debug.LogWarning("SFX not found in Dictionary!");
//        }
//    }
//}