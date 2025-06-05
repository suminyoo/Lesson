using UnityEngine;

public class HammerBall : Trap
{
    private Quaternion startRotation;

    public float swingAngle;
    public float swingSpeed;
    private float bounceSpeed = 7f;
    private float bounceDistance = 6f;

    public void Awake()
    {
        base.damage = 10;
        swingAngle = Random.Range(30f, 80f);  // 회전 각도
        swingSpeed = Random.Range(1f, 4f);    // 속도

    }
    void Start()
    {
        startRotation = transform.localRotation;  // 초기 회전값
    }
    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localRotation = startRotation * Quaternion.Euler(angle, 0, 0);
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySFX(ESfx.Hit);
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);

        StartCoroutine(BounceOffObj(
            collision.gameObject.GetComponent<Player>(), 
            collision.GetContact(0).point, 
            bounceSpeed, 
            bounceDistance));
        base.OnCollisionEnter(collision);

    }
}
