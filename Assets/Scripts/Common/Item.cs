using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;// 위아래 부드럽게 떠다니기
        transform.position = new Vector3(startPos.x, startPos.y + offsetY, startPos.z);
    }
}
