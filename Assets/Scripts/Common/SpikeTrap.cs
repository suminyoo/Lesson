using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private int damage = 10;
    protected override void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Player>().ChangePlayerHP(-damage);
        base.OnCollisionEnter(collision);
    }

}
