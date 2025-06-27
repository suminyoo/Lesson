using UnityEngine;

public class SineVerticalMovement : MonoBehaviour, IMovement
{
    public float speed = 1f;
    public float amplitude = 1f;
    private float startY;
    private bool isStopped = false;

    private void Start()
    {
        startY = transform.position.y;
    }

    public void Move()
    {
        if (isStopped) return;

        float y = startY + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void Stop()
    {
        isStopped = true;
    }
}
