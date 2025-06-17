using UnityEngine;

//확장시 필요

[CreateAssetMenu(menuName = "Effects/EffectData")]
public class EffectDataSO : ScriptableObject
{
    public GameObject prefab;
    public float duration = 1f;
}
