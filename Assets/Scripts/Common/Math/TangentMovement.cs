using UnityEngine;

public class TangentMovement : MonoBehaviour, IMovement
{
    public float speed = 1f;
    public float amplitude = 1f;
    public float maxOffset = 5f;
    private float initialY;
    private bool isStopped = false;

    private void Start()
    {
        initialY = transform.position.y;
    }

    public void Move()
    {
        if (isStopped) return;

        float tanVal = Mathf.Tan(Time.time * speed) * amplitude;
        tanVal = Mathf.Clamp(tanVal, -maxOffset, maxOffset);
        float y = initialY + tanVal;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void Stop()
    {
        isStopped = true;
    }
}
