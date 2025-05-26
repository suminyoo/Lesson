using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public static event Action<Trap> OnAnyTrapTrigger;
    public static event Action<Trap> OnAnyTrapCollision;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnAnyTrapCollision?.Invoke(this);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnAnyTrapTrigger?.Invoke(this);
    }

}