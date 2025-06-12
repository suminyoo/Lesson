using UnityEngine;

public class Blade : Trap
{
    public float speed = 10f;
    public float lifetime = 5f;

    private bool isLaunched = false;
    public void Awake()
    {
        base.damage = 20;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.Hit);
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        base.OnCollisionEnter(collision);
    }

    public void Launch()
    {
        isLaunched = true;
        // 부모에서 분리 (필요하면)
        transform.parent = null;
        Destroy(gameObject, lifetime);  // 일정 시간 후 자동 파괴
    }

    void Update()
    {
        if (isLaunched)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
