using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public MeshRenderer meshR;
    public MeshRenderer meshR02;

    //����� ����Ʈ�� ���� �̺�Ʈ �ۺ��ſ� �����ʵ�
    //SoundManager EffectManager
    public static event Action<Vector3> OnTrapActivateEvent;


    void Start()
    {
        meshR.enabled = false;
        meshR02.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //OnTrapActivateEvent.Invoke(transform.position);
        if (other.gameObject == null) return;
        if(other.gameObject.tag == "Player")
        {
            meshR.enabled = true;
            meshR02.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == null) return;
        if (other.gameObject.tag == "Player")
        {
            meshR.enabled = false;
            meshR02.enabled = false;
        }
    }

}
