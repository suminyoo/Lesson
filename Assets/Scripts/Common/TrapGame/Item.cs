using UnityEngine;

public class Item : MonoBehaviour
{
    public TG_PlayerData playerData;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tag)
            {
                case "Life":
                    other.gameObject.GetComponent<Player>().ChangePlayerHP(20);
                    break;

                case "SpeedUp":
                    playerData.SpeedUpUsable = false;
                    other.gameObject.GetComponent<Player>().ChangePlayerSpeed(playerData.maxSpeed);
                    gameObject.SetActive(false);
                    break;

                case "JumpUp":
                    playerData.JumpPowUpUsable = false;
                    other.gameObject.GetComponent<Player>().ChangePlayerJumpPow(playerData.maxJumpPow);
                    gameObject.SetActive(false);
                    break;

                default:
                    Debug.Log("Unknown Item");
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
