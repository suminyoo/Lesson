using System.Collections;
using UnityEngine;
public class SpikeTrap : Trap
{
    public void Awake()
    {
        base.damage = 10;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayOneList(AudioType.Hit);
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        base.OnCollisionEnter(collision);
    }

}
