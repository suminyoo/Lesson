using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;            // 따라갈 캐릭터
    public Vector3 offset = new Vector3(0, 2, -5);  // 카메라 거리
    public float mouseSensitivity = 2f;
    public float distance = 5f;         // 타겟으로부터 거리
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
        // 마우스 입력
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle); // 상하 제한

        // 회전 계산
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 direction = rotation * Vector3.back;

        // 최종 위치 계산
        transform.position = target.position + direction * distance + Vector3.up * offset.y;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}