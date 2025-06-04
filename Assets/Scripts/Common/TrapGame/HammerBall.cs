using UnityEngine;

public class HammerBall : Trap
{
    private Quaternion startRotation;

    public float swingAngle = 50f;
    public float swingSpeed = 2f;
    private float bounceSpeed = 7f;
    private float bounceDistance = 6f;

    public void Awake()
    {
        base.damage = 10;
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
        SoundManager.Instance.PlayOneList(AudioType.Hit);
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        Vector3 contact = collision.GetContact(0).point;

        StartCoroutine(BounceOffObj(collision.gameObject.GetComponent<Player>(), contact, bounceSpeed, bounceDistance));
        base.OnCollisionEnter(collision);

    }
}
