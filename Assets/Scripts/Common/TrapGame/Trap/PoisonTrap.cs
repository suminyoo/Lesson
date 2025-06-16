using UnityEngine;

public class PoisonTrap : Trap
{
    [SerializeField] private EffectEventSO effectEvent;

    public TG_PlayerData playerData;

    public float tickInterval = 1.0f;
    private float tickTimer = 0f;
    private float deBuffedSpeed = 2f;
    private float deBuffedJumpPow = 2f;

    public void Awake()
    {
        base.damage = 5;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySFX(ESfx.PowerDown);
        other.GetComponent<Player>().ChangePlayerSpeed(deBuffedSpeed);
        other.GetComponent<Player>().ChangePlayerJumpPow(deBuffedJumpPow);
        base.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!playerData.isDead)
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0f)
            {
                SoundManager.Instance.PlaySFX(ESfx.Hit);
                //EffectManager.Instance.PlayEffect(EEffect.Hit, other.gameObject.GetComponent<Player>().transform.position);
                effectEvent.Raise(EEffect.Hit, other.gameObject.GetComponent<Player>().transform.position);

                tickTimer = tickInterval;
                other.GetComponent<Player>().ChangePlayerHP(-damage);
                base.OnTriggerEnter(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        tickTimer = 0f;
        other.GetComponent<Player>().ChangePlayerSpeed(playerData.normalSpeed);
        other.GetComponent<Player>().ChangePlayerJumpPow(playerData.normalJumpPow);
    }
}
