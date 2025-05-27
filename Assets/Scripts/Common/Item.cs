using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotationSpeed = 50f;      // 회전 속도 (도/초)
    public float floatAmplitude = 0.25f;   // 위아래 움직이는 폭
    public float floatFrequency = 1f;      // 위아래 움직이는 속도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 위아래 부드럽게 떠다니기
        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + offsetY, startPos.z);
    }
}
