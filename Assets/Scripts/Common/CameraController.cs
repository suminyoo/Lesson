using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;            // ���� ĳ����
    public Vector3 offset = new Vector3(0, 2, -5);  // ī�޶� �Ÿ�
    public float mouseSensitivity = 2f;
    public float distance = 5f;         // Ÿ�����κ��� �Ÿ�
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // ���콺 �Է�
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle); // ���� ����

        // ȸ�� ���
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 direction = rotation * Vector3.back;

        // ���� ��ġ ���
        transform.position = target.position + direction * distance + Vector3.up * offset.y;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}