using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 2f;
    public Vector3 moveAxis = Vector3.right;

    private Vector3 startPos;
    private Vector3 lastPosition;
    private float timeOffset;

    private HashSet<Transform> passengers = new HashSet<Transform>();

    void Start()
    {
        startPos = transform.position;
        lastPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed + timeOffset) * moveDistance;
        Vector3 newPosition = startPos + moveAxis.normalized * offset;

        Vector3 delta = newPosition - lastPosition;

        // 위에 있는 플레이어 이동
        foreach (Transform t in passengers)
        {
            t.position += delta;
        }

        transform.position = newPosition;
        lastPosition = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            passengers.Add(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            passengers.Remove(collision.transform);
        }
    }
}
