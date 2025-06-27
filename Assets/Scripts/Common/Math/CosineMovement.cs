using UnityEngine;

public class CosineMovement : MonoBehaviour, IMovement
{
    public float speed = 1f;
    public float amplitude = 1f;
    private float initialX;
    private bool isStopped = false;

    private void Start()
    {
        initialX = transform.position.x;
    }

    public void Move()
    {
        if (isStopped) return;

        float x = initialX + Mathf.Cos(Time.time * speed) * amplitude;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void Stop()
    {
        isStopped = true;
    }
}
