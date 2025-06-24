using System.Collections;
using UnityEngine;
public class SpikeTrap : Trap
{
    [SerializeField] private EffectEventSO effectEvent;

    public void Awake()
    {
        base.damage = 10;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.Hit);
        //EffectManager.Instance.PlayEffect(EEffect.Hit, collision.gameObject.GetComponent<Player>().transform.position);
        effectEvent.Raise(EEffect.Hit, collision.gameObject.GetComponent<Player>().transform.position);

        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        base.OnCollisionEnter(collision);
    }
}
