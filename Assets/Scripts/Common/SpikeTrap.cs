using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap
{

    [SerializeField] private int damage = 10;

    protected override void TrapCollision(Player player)
    {
        Debug.Log("Spike " + damage);
        player.ChangePlayerHP(-damage);
        base.TrapCollision(player);
    }
}
