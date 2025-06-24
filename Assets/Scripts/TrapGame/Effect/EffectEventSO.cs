using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Effects/EffectEvent")]
public class EffectEventSO : ScriptableObject
{
    public Action<EEffect, Vector3> OnRequested;

    public void Raise(EEffect effectType, Vector3 position)
    {
        OnRequested?.Invoke(effectType, position);
    }
}
