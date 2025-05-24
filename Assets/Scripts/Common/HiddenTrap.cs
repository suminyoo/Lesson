using System;
using UnityEngine;

public class HiddenTrap : Trap
{
    //사운드와 이펙트를 위한 이벤트 퍼블리셔와 리스너들
    //SoundManager EffectManager
    //public static event Action<Vector3> OnTrapActivateEvent;


    [SerializeField] private MeshRenderer meshR;
    [SerializeField] private MeshRenderer meshR02;

    private void Start()
    {
        meshR.enabled = false;
        meshR02.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.enabled = true;
            meshR02.enabled = true;
        }
        if (other.TryGetComponent<Player>(out var player))
        {
            TrapTrigger(player);
            base.TrapTrigger(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.enabled = false;
            meshR02.enabled = false;
        }
    }

}
