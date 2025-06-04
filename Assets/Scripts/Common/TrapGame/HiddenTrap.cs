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
            SoundManager.Instance.PlayOneList(AudioType.Hit);
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
        }
    }

}
