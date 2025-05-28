using System;
using System.Collections;
using UnityEngine;

public class BombTrap : HiddenTrap
{
    [SerializeField] private int damage = 50;
    public float duration = 1.5f;
    public float bounceForce = 10f;

    protected override void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        Vector3 contact = collision.GetContact(0).point;

        StartCoroutine(BounceOffObj(collision.gameObject.GetComponent<Player>(), contact));
        base.OnCollisionEnter(collision);

    }

}
