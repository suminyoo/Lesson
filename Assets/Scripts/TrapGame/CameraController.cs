using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);

    public float mouseSensitivity = 2f;
    public float distance = 5f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;
    private float currentYaw = 90f;
    private float currentPitch = 20f;

    private bool isPaused = false;

    void Start()
    {
        GameManager.OnPaused += Pause;
    }
    private void Pause(bool boo)
    {
        isPaused = boo;
        SetCursorVisible(boo);
    }
    public void ResetYaw(float newYaw)
    {
        currentYaw = newYaw;
    }
    public void SetCursorVisible(bool isVisible)
    {
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void LateUpdate()
    {
        if (isPaused) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle); //상하 제한

        // 회전 계산
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 direction = rotation * Vector3.back;

        // 최종 위치 계산
        transform.position = target.position + direction * distance + Vector3.up * offset.y;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}