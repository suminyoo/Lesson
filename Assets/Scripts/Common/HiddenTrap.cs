using System;
using UnityEngine;

public class HiddenTrap : Trap
{
    //����� ����Ʈ�� ���� �̺�Ʈ �ۺ��ſ� �����ʵ�
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
