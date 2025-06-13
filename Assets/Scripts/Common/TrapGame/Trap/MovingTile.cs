using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 2f;
    public Vector3 moveAxis = Vector3.right;

    private Vector3 startPos;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // 움직임 비동기화
    }
    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed + timeOffset) * moveDistance;
        transform.position = startPos + moveAxis.normalized * offset;
    }
}