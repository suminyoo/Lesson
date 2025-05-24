using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static event Action<Player, GameObject> OnPlayerCollisionEventWithObj;
    public static event Action<Player, GameObject> OnPlayerTriggerEventWithObj;

    public static event Action OnPlayerDie;

    public Animator anim;
    public Rigidbody rigid;
    public Transform cameraTransform;

    public float turnSpeed = 0.05f; // 더 작을수록 느림
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;

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
        playerRespawnPosition = new Vector3(7, 5, 12);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();


    }
    //private void OnPlayerTrapCollisionEvent(Player player, Trap trap)
    //{
    //    OnPlayerCollisionEventWithObj?.Invoke(player, trap.gameObject);

    //}
    //private void OnPlayerTrapTriggerEvent(Player player, Trap trap)
    //{
    //    OnPlayerTriggerEventWithObj?.Invoke(player, trap.gameObject);

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;

        if (collision.gameObject.CompareTag("Life")) ChangePlayerHP(20);

        if (collision == null) { return; }


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

    }



    void FixedUpdate()
    {
        PlayerMovement();
    }
    public void OnDie()
    {
        life -= 1;
        hp = 100;
        transform.position = playerRespawnPosition;
    }

    public void ChangePlayerHP(int var)
    {
        Debug.Log("ChangePlayerHP: " + hp);
        hp += var;
        if (hp > 100)
        {
            hp = 100;
        }
        if (hp <= 0)
        {
            OnDie();
            OnPlayerDie.Invoke();

        }

    }

    public void ChangePlayerSpeed(float speed)
    {
        moveSpeed = speed;
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
