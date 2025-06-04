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
        SoundManager.Instance.PlayOneList(AudioType.BombExplode);
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        Vector3 contact = collision.GetContact(0).point;

        StartCoroutine(BounceOffObj(collision.gameObject.GetComponent<Player>(), contact, bounceSpeed, bounceDistance));
        base.OnCollisionEnter(collision);
    }
}
