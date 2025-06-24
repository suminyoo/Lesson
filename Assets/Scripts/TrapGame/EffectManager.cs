using System.Collections.Generic;
using UnityEngine;

public enum EEffect
{
    Hit,
    BombExplode,
    Respawn,
    PowerUp,
    //Clear,
    //PowerDown
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [Header("Effect Prefabs")]
    private Dictionary<EEffect, GameObject> effectDict;

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
        effectDict = new Dictionary<EEffect, GameObject>();
        foreach (EEffect effect in System.Enum.GetValues(typeof(EEffect)))
        {
            var prefab = Resources.Load<GameObject>($"Effects/{effect}");
            if (prefab != null)
                effectDict[effect] = prefab;
            else
                Debug.LogWarning($"Effect prefab not found: {effect}");
        }
    }

    public void PlayEffect(EEffect effectType, Vector3 position, float duration = 2f)
    {
        if (effectDict.TryGetValue(effectType, out var prefab))
        {
            GameObject instance = Instantiate(prefab, position, Quaternion.identity);
            Destroy(instance, duration);
        }
        else
        {
            Debug.LogWarning("Effect prefab not found in Dictionary!");
        }
    }
}
