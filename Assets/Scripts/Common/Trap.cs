using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public static event Action<Player, Trap> OnAnyTrapTrigger;
    public static event Action<Player, Trap> OnAnyTrapCollision;
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            TrapCollision(player);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            TrapTrigger(player);
        }
    }

    protected virtual void TrapCollision(Player player) //필수 호출
    {
        Debug.Log("Trap Collision" + this);
        OnAnyTrapCollision?.Invoke(player, this);
    }
    protected virtual void TrapTrigger(Player player) //필수 호출
    {
        Debug.Log("Trap triggered" + this);
        OnAnyTrapTrigger?.Invoke(player, this);
    }



}