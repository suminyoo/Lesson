using System;
using UnityEngine;

public class BombTrap : HiddenTrap
{
    [SerializeField] private int damage = 100;

    protected override void TrapCollision(Player player)
    {

        Debug.Log("Bomb " + damage);

        player.ChangePlayerHP(-damage);
        base.TrapCollision(player);
    }
}
