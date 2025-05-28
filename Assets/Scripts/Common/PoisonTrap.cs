using System;
using System.Collections;
using UnityEngine;

public class PoisonTrap : Trap
{
    public float tickInterval = 1.0f;
    private float tickTimer = 0f;


    public void Awake()
    {
        base.damage = 5;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        //속도가 다른 상태에서 들어오면???
        other.GetComponent<Player>().ChangePlayerSpeed(2);
        other.GetComponent<Player>().ChangePlayerJumpPow(2);
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
        other.GetComponent<Player>().ChangePlayerJumpPow(5);

    }


}
