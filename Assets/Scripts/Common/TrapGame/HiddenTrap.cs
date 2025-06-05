using System.Collections;
using UnityEngine;
public class HiddenTrap : Trap
{
    [SerializeField] private MeshRenderer meshR;
    [SerializeField] private MeshRenderer meshR02;

    private void Start()
    {
        meshR.enabled = false;
        meshR02.enabled = false;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tag == "Bomb")
                InvokeRepeating("TickSound", 0, 1);

            meshR.enabled = true;
            meshR02.enabled = true;
        }
        base.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.enabled = false;
            meshR02.enabled = false;
            
            if(tag == "Bomb")
                CancelInvoke("TickSound");
        }
    }
    private void TickSound()
    {
        Debug.Log("tickSound");
        SoundManager.Instance.PlaySFX(ESfx.BombVisible);
    }


}
