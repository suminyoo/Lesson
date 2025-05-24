using System;
using System.Collections;
using UnityEngine;

public class PoisonTrap : Trap
{
    public int damage = 5;
    public float tickInterval = 1.0f;

    private float tickTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            tickTimer -= Time.deltaTime;

            if (tickTimer <= 0f)
            {
                TrapTrigger(player);
                tickTimer = tickInterval;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tickTimer = 0f; // 빠져나가면 즉시 리셋
        }
    }

    protected override void TrapTrigger(Player player)
    {
        Debug.Log("PoisonTrap " + damage);

        player.ChangePlayerHP(-damage);
        base.TrapTrigger(player);
    }



}
