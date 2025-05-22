using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public MeshRenderer meshR;
    public MeshRenderer meshR02;

    //사운드와 이펙트를 위한 이벤트 퍼블리셔와 리스너들
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
