using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TG_PlayerData playerData;

    public static event Action<GameObject> OnPlayerCollisionEventWithObj;
    public static event Action<GameObject> OnPlayerTriggerEventWithObj;

    public static event Action<float, float> OnPlayerSpeedChangeEvent;
    public static event Action<float, float> OnPlayerJumpPowChangeEvent;

    public static event Action OnStageClear;
    public static event Action OnPlayerDie;

    public Animator anim;
    public Rigidbody rigid;
    public Transform cameraTransform;

    public bool isGrounded;
    public bool isPaused = false;

    private float h;
    private float v;

    private void Awake()
    {
        InitializePlayer();
    }
    public void InitializePlayer()
    {
        playerData.hp = 100;
        playerData.life = 3;
        RespawnPlayer();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        GameManager.OnPaused += Pause;
    }
    private void Pause(bool boo)
    {
        isPaused = boo;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) { return; }

        OnPlayerCollisionEventWithObj?.Invoke(collision.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Life"))
        {
            ChangePlayerHP(20);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Clear"))
        {
            OnStageClear.Invoke();
        }
        else if (other.gameObject.CompareTag("SpeedUp"))
        {
            playerData.SpeedUpUsable = false;
            ChangePlayerSpeed(playerData.maxSpeed);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("JumpUp"))
        {
            playerData.JumpPowUpUsable = false;
            ChangePlayerJumpPow(playerData.maxJumpPow);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("DeathArea"))
        {
            OnDie();
        }
        OnPlayerTriggerEventWithObj?.Invoke(other.gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // 접촉 면이 위쪽을 향할수록 dot 값이 1에 가까움 (0.5 이상이면 수평면)
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.CompareTag("Ground"))
                {
                    Debug.Log("Staying in Ground");
                    isGrounded = true;
                    return;
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void OnDie()
    {
        playerData.life -= 1;
        playerData.hp = 100;
        OnPlayerDie.Invoke();
    }
    public void RespawnPlayer()
    {
        transform.position = playerData.playerRespawnPosition;
    }
    public void ChangePlayerHP(int var)
    {
        playerData.hp += var;
        if (playerData.hp > 100) playerData.hp = 100;
        if (playerData.hp <= 0) OnDie();
    }
    public void ChangePlayerSpeed(float speed)
    {
        playerData.moveSpeed = speed;
        OnPlayerSpeedChangeEvent.Invoke(playerData.normalSpeed, speed);
    }
    public void ChangePlayerJumpPow(float jumpPow)
    {
        playerData.jumpPower = jumpPow;
        OnPlayerJumpPowChangeEvent.Invoke(playerData.normalJumpPow, jumpPow);
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            anim.SetTrigger("Attack01");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("Attack04");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector3.up * playerData.jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
        if (!playerData.SpeedUpUsable)
        {
            playerData.speedRemainTime -= Time.deltaTime;
            if (playerData.speedRemainTime < 0)
            {
                playerData.speedRemainTime = 5f;
                playerData.SpeedUpUsable = true;
                ChangePlayerSpeed(playerData.normalSpeed);
            }
        }
        if (!playerData.JumpPowUpUsable)
        {
            playerData.JumpPowRemainTime -= Time.deltaTime;
            if (playerData.JumpPowRemainTime < 0)
            {
                playerData.JumpPowRemainTime = 5f;
                playerData.JumpPowUpUsable = true;
                ChangePlayerJumpPow(playerData.normalJumpPow);
            }
        }
    }
    void FixedUpdate()
    {
        if (isPaused) return;
        PlayerMovement();
    }
    private void PlayerMovement()
    {

        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        if (inputDir.magnitude < 0.1f)
        {
            anim.SetBool("Walk", false);
            return;
        }
        // 카메라 기준 방향 계산
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 이동 방향을 카메라 기준으로 변환
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        // 회전 (부드럽게)
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, playerData.turnSpeed);
        rigid.MoveRotation(smoothedRotation);

        // 이동
        Vector3 moveAmount = moveDir * playerData.moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveAmount);

        // 애니메이션
        anim.SetBool("Walk", isGrounded);
    }
}
