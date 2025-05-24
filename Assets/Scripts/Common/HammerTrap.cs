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
            // �浹�� ���� �� ù ��°�� ���
            Vector3 contact = collision.GetContact(0).point;

            StartCoroutine(BounceOffObj(player, contact));
            TrapCollision(player);
        }
    }
    public IEnumerator BounceOffObj(Player player, Vector3 contactPoint)
    {
        float bounceSpeed = 7f;       // �ӵ�: �ʴ� 20 ���� �Ÿ��� ƨ��
        float bounceDistance = 6f;     // �� 2 ���ָ�ŭ �з���

        // 1. ƨ�� ���� ���: �浹 ��ġ���� �ݴ� + ���� ��¦
        Vector3 bounceDir = (player.transform.position - contactPoint).normalized + Vector3.up;
        bounceDir = bounceDir.normalized; // ���� 1�� ����ȭ�ؼ� ���⸸ ����

        // 2. �ӵ��κ��� �����Ӻ� �̵��� ��� �غ�
        Vector3 velocity = bounceDir * bounceSpeed;
        Vector3 start = player.transform.position;
        float traveled = 0f;

        // 3. �̵� ����: ���� �Ÿ� < ��ǥ �Ÿ��� ���� �ݺ�
        while (traveled < bounceDistance)
        {
            float step = bounceSpeed * Time.deltaTime; // �̹� �����ӿ��� �̵��� �Ÿ�
            player.transform.position += bounceDir * step; // �׸�ŭ �̵�
            traveled += step; // �� �̵� �Ÿ� ����

            yield return null; // ���� �����ӱ��� ��ٸ�
        }
    }
}