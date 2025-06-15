using UnityEngine;

public class Blade : Trap
{
    private float speed;
    private float lifetime;
    private int _damage;
    private bool isLaunched;

    public void Initialize(float speed, float lifetime, int damage)
    {
        this.speed = speed;
        this.lifetime = lifetime;
        this._damage = damage;
        isLaunched = true;

        Destroy(gameObject, lifetime);  // 일정 시간 후 파괴
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.Hit);
        EffectManager.Instance.PlayEffect(EEffect.Hit, collision.gameObject.GetComponent<Player>().transform.position);

        collision.gameObject.GetComponent<Player>()?.ChangePlayerHP(-_damage);
        base.OnCollisionEnter(collision);
    }

    void Update()
    {
        if (isLaunched)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
