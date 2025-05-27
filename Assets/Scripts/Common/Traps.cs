using System;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public static event Action<Traps> OnAnyTrapTrigger;
    public static event Action<Traps> OnAnyTrapCollision;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnAnyTrapCollision?.Invoke(this as Traps);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnAnyTrapTrigger?.Invoke(this as Traps);
    }
    
}
