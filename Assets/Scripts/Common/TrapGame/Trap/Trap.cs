using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public static event Action<string> OnAnyTrapTrigger;
    public static event Action<string> OnAnyTrapCollision;

    public int damage;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnAnyTrapCollision?.Invoke(this.gameObject.name);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnAnyTrapTrigger?.Invoke(this.gameObject.name);
    }
    public IEnumerator BounceOffObj(Player player, Vector3 contactPoint, float bounceSpeed, float bounceDistance)
    {
        Vector3 bounceDir = (player.transform.position - contactPoint).normalized + Vector3.up;
        bounceDir = bounceDir.normalized;

        Vector3 velocity = bounceDir * bounceSpeed;
        Vector3 start = player.transform.position;
        float traveled = 0f;

        while (traveled < bounceDistance) //누적 거리 < 목표 거리일 동안 반복
        {
            float step = bounceSpeed * Time.deltaTime; // 이번 프레임에서 이동할 거리
            player.transform.position += bounceDir * step; // 이동
            traveled += step; // 누적
            yield return null;
        }
    }
}
