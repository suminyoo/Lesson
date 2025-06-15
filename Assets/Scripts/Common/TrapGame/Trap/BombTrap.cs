using UnityEngine;

public class BombTrap : HiddenTrap
{
    private float bounceSpeed = 10f;
    private float bounceDistance = 10f;

    public void Awake()
    {
        base.damage = 50;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.BombExplode);
        EffectManager.Instance.PlayEffect(EEffect.BombExplode, transform.position);
        EffectManager.Instance.PlayEffect(EEffect.Hit, collision.gameObject.GetComponent<Player>().transform.position);

        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);

        StartCoroutine(BounceOffObj(
            collision.gameObject.GetComponent<Player>(), 
            collision.GetContact(0).point, 
            bounceSpeed, 
            bounceDistance));
        base.OnCollisionEnter(collision);
    }
}
