using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnStageClear;

    [SerializeField] TG_PlayerData playerData;
    [SerializeField] private EffectEventSO effectEvent;

    public event Action<float> OnPlayerSpeedChangeEvent;
    public event Action<float> OnPlayerJumpPowChangeEvent;

    private Animator anim;
    private Rigidbody rigid;

    [SerializeField] Transform cameraTransform;

    private bool isGrounded;
    private bool isPaused = false;

    private float h;
    private float v;

    public void InitializePlayer()
    {
        playerData.Reset();
        RespawnPlayer();
    }
    private void Start()
    {
        playerData.OnLifeLost += HandleLifeLost;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        GameManager.OnPaused += Pause;

        InitializePlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerData.isDead) return;
        if (other.gameObject.CompareTag("Clear")) OnStageClear.Invoke();
        if (other.gameObject.CompareTag("DeathArea"))
        {
            playerData.DeathReason = "Killed By DeathArea";
            playerData.ChangeHP(-100);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f) // 접촉 면이 위쪽을 향할수록 dot 값이 1에 가까움 (0.5 이상이면 수평면)
            {
                if (collision.gameObject.CompareTag("Ground"))
                {
                    isGrounded = true;
                    return;
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
    public void HandleLifeLost()
    {
        rigid.isKinematic = true;
        playerData.isDead = true;
    }
    public void RespawnPlayer()
    {
        GameObject[] blades = GameObject.FindGameObjectsWithTag("Blade");
        foreach (GameObject blade in blades)
        {
            Destroy(blade);
        }

        rigid.isKinematic = false;

        playerData.isDead = false;
        playerData.Respawn();
        ChangePlayerSpeed(playerData.normalSpeed);
        ChangePlayerJumpPow(playerData.normalJumpPow);

        transform.position = playerData.playerRespawnPosition;
        transform.rotation = Quaternion.LookRotation(Vector3.right);

        if (rigid != null)
        {
            rigid.linearVelocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.rotation = Quaternion.LookRotation(Vector3.right);
        }
        cameraTransform.GetComponent<CameraController>().ResetYaw(90f);
        //EffectManager.Instance.PlayEffect(EEffect.Respawn, transform.position);

        effectEvent.Raise(EEffect.Respawn, transform.position);



    }
    private void Pause(bool boo) => isPaused = boo;
    public void ChangePlayerHP(int var) => playerData.ChangeHP(var); 
    public void ChangePlayerSpeed(float speed)
    {
        playerData.moveSpeed = speed;
        if(OnPlayerSpeedChangeEvent != null)
            OnPlayerSpeedChangeEvent.Invoke(speed);
    }
    public void ChangePlayerJumpPow(float jumpPow)
    {
        playerData.jumpPower = jumpPow;
        if (OnPlayerJumpPowChangeEvent != null)
            OnPlayerJumpPowChangeEvent.Invoke(jumpPow);
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Z)) anim.SetTrigger("Attack01");
        if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("Attack04");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            SoundManager.Instance.PlaySFX(ESfx.Jump);
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector3.up * playerData.jumpPower, ForceMode.Impulse);
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
    private void FixedUpdate()
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
        Vector3 camForward = cameraTransform.forward;  // 카메라 기준 방향 계산
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x; // 이동 방향을 카메라 기준으로 변환

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, playerData.turnSpeed);         
        rigid.MoveRotation(smoothedRotation); // 회전 (부드럽게)

        Vector3 moveAmount = moveDir * playerData.moveSpeed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + moveAmount);  // 이동

        anim.SetBool("Walk", isGrounded); // 애니메이션
        //SoundManager.Instance.PlayOneList(AudioType.Walk);

    }
}
