using System;
using System.Collections;
using UnityEngine;

public class PoisonTrap : Trap
{
    public int damage = 5;
    public float tickInterval = 1.0f;

    private float tickTimer = 0f;
    protected override void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().ChangePlayerSpeed(2);
        base.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0f)
        {
            tickTimer = tickInterval;
            other.GetComponent<Player>().ChangePlayerHP(-damage);
            base.OnTriggerEnter(other);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        tickTimer = 0f; // 빠져나가면 리셋
        other.GetComponent<Player>().ChangePlayerSpeed(5);
    }


}
