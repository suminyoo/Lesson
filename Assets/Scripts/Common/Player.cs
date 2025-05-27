using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerCollisionEventWithObj;
    public static event Action<GameObject> OnPlayerTriggerEventWithObj;

    public static event Action<float, float> OnPlayerSpeedChangeEvent;
    public static event Action<float, float> OnPlayerJumpPowChangeEvent;


    public static event Action OnGameClear;

    public static event Action OnPlayerDie;

    public Animator anim;
    public Rigidbody rigid;
    public Transform cameraTransform;

    public float turnSpeed = 0.05f; // 더 작을수록 느림
    public float moveSpeed = 5f;
    public float jumpPower = 5f;

    public float maxSpeed = 8f;
    public float maxJumpPow = 8f;

    public float normalSpeed = 5f;
    public float normalJumpPow = 5f;

    public float speedRemainTime = 5f;
    public float JumpPowRemainTime = 5f;

   
    bool SpeedUpUsable;
    bool JumpPowUpUsable;


    public bool isGrounded = true;

    private float h;
    private float v;

    public int hp;
    public int life;

    Vector3 playerRespawnPosition;

    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        hp = 100;
        life = 3;
        playerRespawnPosition = new Vector3(0, 2.5f, 0);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;

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
            OnGameClear.Invoke();
        }
        else if (other.gameObject.CompareTag("SpeedUp"))
        {
            SpeedUpUsable = false;
            ChangePlayerSpeed(maxSpeed);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("JumpUp"))
        {
            JumpPowUpUsable = false;
            ChangePlayerJumpPow(maxJumpPow);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("DeathArea"))
        {
            OnDie();
        }

        OnPlayerTriggerEventWithObj?.Invoke(other.gameObject);


    }
    public void OnDie()
    {
        life -= 1;
        hp = 100;
        OnPlayerDie.Invoke();
        transform.position = playerRespawnPosition;
    }
    public void ChangePlayerHP(int var)
    {
        hp += var;
        if (hp > 100) hp = 100;
        if (hp <= 0) OnDie();
    }
    public void ChangePlayerSpeed(float speed)
    {
        moveSpeed = speed;
        OnPlayerSpeedChangeEvent.Invoke(normalSpeed, speed);

    }
    public void ChangePlayerJumpPow(float jumpPow)
    {
        jumpPower = jumpPow;
        OnPlayerJumpPowChangeEvent.Invoke(normalJumpPow, jumpPow);

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
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }

        if (!SpeedUpUsable)
        {
            speedRemainTime -= Time.deltaTime;
            //Debug.Log("speedRemainTime "+ speedRemainTime +" Time.deltaTime" +  Time.deltaTime);
            if (speedRemainTime < 0)
            {
                speedRemainTime = 5f;
                SpeedUpUsable = true;
                ChangePlayerSpeed(normalSpeed);

            }
        }
        if (!JumpPowUpUsable)
        {
            JumpPowRemainTime -= Time.deltaTime;
            //Debug.Log("JumpPowRemainTime "+ JumpPowRemainTime +" Time.deltaTime" +  Time.deltaTime);

            if (JumpPowRemainTime < 0)
            {
                JumpPowRemainTime = 5f;
                JumpPowUpUsable = true;
                ChangePlayerJumpPow(normalJumpPow);
            }
        }

    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {

        //플레이어 이동
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude < 0.1f)
        {
            anim.SetBool("Walk", false);
            return;
        }

        // 1. 카메라 기준 방향 계산
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 2. 이동 방향을 카메라 기준으로 변환
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        // 3. 회전 (부드럽게)
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
        rigid.MoveRotation(smoothedRotation);

        // 4. 이동
        Vector3 moveAmount = moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveAmount);

        // 5. 애니메이션 처리
        anim.SetBool("Walk", true);
    }
   
}
