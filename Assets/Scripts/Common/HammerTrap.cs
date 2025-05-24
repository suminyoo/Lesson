using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HammerTrap : Trap
{
    [SerializeField] private int damage = 10;
    public float duration = 1.5f;

    protected override void TrapCollision(Player player)
    {
        Debug.Log("HammerTrap " + damage);

        player.ChangePlayerHP(-damage);
        base.TrapCollision(player);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            // 충돌한 지점 중 첫 번째를 사용
            Vector3 contact = collision.GetContact(0).point;

            StartCoroutine(BounceOffObj(player, contact));
            TrapCollision(player);
        }
    }
    public IEnumerator BounceOffObj(Player player, Vector3 contactPoint)
    {
        float bounceSpeed = 7f;       // 속도: 초당 20 유닛 거리로 튕김
        float bounceDistance = 6f;     // 총 2 유닛만큼 밀려남

        // 1. 튕길 방향 계산: 충돌 위치에서 반대 + 위로 살짝
        Vector3 bounceDir = (player.transform.position - contactPoint).normalized + Vector3.up;
        bounceDir = bounceDir.normalized; // 길이 1로 정규화해서 방향만 남김

        // 2. 속도로부터 프레임별 이동량 계산 준비
        Vector3 velocity = bounceDir * bounceSpeed;
        Vector3 start = player.transform.position;
        float traveled = 0f;

        // 3. 이동 루프: 누적 거리 < 목표 거리일 동안 반복
        while (traveled < bounceDistance)
        {
            float step = bounceSpeed * Time.deltaTime; // 이번 프레임에서 이동할 거리
            player.transform.position += bounceDir * step; // 그만큼 이동
            traveled += step; // 총 이동 거리 누적

            yield return null; // 다음 프레임까지 기다림
        }
    }
}