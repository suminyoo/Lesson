using System.Collections;
using UnityEngine;
public class HiddenTrap : Trap
{
    [SerializeField] private MeshRenderer meshR;
    [SerializeField] private MeshRenderer meshR02;

    private bool isTicking = false;
    private Coroutine tickingCoroutine;

    private void Start()
    {
        meshR.enabled = false;
        meshR02.enabled = false;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.enabled = true;
            meshR02.enabled = true;
        }
        base.OnTriggerEnter(other);
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isTicking)
        {
            tickingCoroutine = StartCoroutine(BombTickSound());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.enabled = false;
            meshR02.enabled = false;

            if (isTicking)
            {
                StopCoroutine(tickingCoroutine);
                isTicking = false;
            }
        }
    }

    private IEnumerator BombTickSound()
    {
        isTicking = true;
        while (true)
        {
            SoundManager.Instance.PlaySFX(ESfx.BombVisible);
            yield return new WaitForSeconds(1f); // Æ½ °£°Ý
        }
    }

}
