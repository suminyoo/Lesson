using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private EffectEventSO effectEvent;

    private Dictionary<EEffect, GameObject> effectCache = new Dictionary<EEffect, GameObject>();

    private void Start()
    {
        effectEvent.OnRequested += HandleEffect;
    }

    private void HandleEffect(EEffect effectType, Vector3 position)
    {

        if (!effectCache.TryGetValue(effectType, out GameObject prefab))
        {
            prefab = Resources.Load<GameObject>($"Effects/{effectType}");
            if (prefab != null)
                effectCache[effectType] = prefab;
            else
                Debug.LogWarning($"Effect prefab not found in Resources/Effects: {effectType}");
        }

        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, position, Quaternion.identity);
            Destroy(instance, 2f);
        }

    }


}
