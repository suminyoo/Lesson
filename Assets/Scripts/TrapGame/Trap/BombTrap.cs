using UnityEngine;

public class BombTrap : HiddenTrap
{
    [SerializeField] private EffectEventSO effectEvent;

    private float bounceSpeed = 10f;
    private float bounceDistance = 10f;

    public void Awake()
    {
        base.damage = 50;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.BombExplode);
        effectEvent.Raise(EEffect.BombExplode, transform.position);
        effectEvent.Raise(EEffect.Hit, collision.gameObject.GetComponent<Player>().transform.position);

        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);

        StartCoroutine(BounceOffObj(
            collision.gameObject.GetComponent<Player>(), 
            collision.GetContact(0).point, 
            bounceSpeed, 
            bounceDistance));
        base.OnCollisionEnter(collision);
    }
}
