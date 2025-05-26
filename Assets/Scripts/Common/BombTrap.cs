using System;
using UnityEngine;

public class BombTrap : HiddenTrap
{
    [SerializeField] private int damage = 100;

    protected override void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        base.OnCollisionEnter(collision);
    }
}
