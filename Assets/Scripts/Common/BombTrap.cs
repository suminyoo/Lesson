using System;
using System.Collections;
using UnityEngine;

public class BombTrap : HiddenTrap
{
    public float duration = 1.5f;
    public float bounceForce = 10f;
    public void Awake()
    {
        base.damage = 50;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        Vector3 contact = collision.GetContact(0).point;

        StartCoroutine(BounceOffObj(collision.gameObject.GetComponent<Player>(), contact));
        base.OnCollisionEnter(collision);

    }

}
