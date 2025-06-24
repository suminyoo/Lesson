using UnityEngine;

public class FallingTile : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.1f;
    public float fallDelay = 0.5f;
    public float fallSpeed = 6f;
    public float destroyDelay = 3f;

    private Vector3 originalLocalPosition;
    private bool isTriggered = false;
    private float timer = 0f;
    private float fallTimer = 0f;
    private bool isShaking = false;
    private bool isFalling = false;

    private void Start()
    {
        // 부모 기준으로 흔들리게 하기 위해 localPosition
        originalLocalPosition = transform.localPosition;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Vector3 center = boxCollider.center;
        center.y += 0.1f;
        boxCollider.center = center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            isShaking = true;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (isShaking)
        {
            timer += Time.deltaTime;

            // 진동을 localPosition으로만 표현 (시각 효과)
            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = originalLocalPosition + new Vector3(shakeOffset.x, 0f, shakeOffset.y);

            if (timer >= shakeDuration)
            {
                isShaking = false;
                isFalling = true;
                fallTimer = 0f;

                // 진동 끝나면 위치 원래대로
                transform.localPosition = originalLocalPosition;
            }
        }
        else if (isFalling)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallDelay)
            {
                // 실제로 떨어질 땐 position을 사용 (world 기준)
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}