using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //충돌시 이벤트발생: Action<GameObject> 만들어서 invoke();
    //어떤 오브젝트에 충돌했는지 알기 위함
    public static event Action OnPlayerCollisionEvent;
    public static event Action<GameObject> OnPlayerCollisionEventWithObj;

    public static event Action OnPlayerTriggerEvent;
    public static event Action<GameObject> OnPlayerTriggerEventWithObj;

    public Animator anim;
    public Rigidbody rigid;
    public Transform cameraTransform;

    public float turnSpeed = 0.05f; // 더 작을수록 느림
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;

    public bool isGrounded = true;

    private float h;
    private float v;

    public int hp = 100;
    public int life = 3;



    private void OnCollisionEnter(Collision collision) //부딪혔을때 캐릭터나 충돌체에 영향이 갈때
    {
        OnPlayerCollisionEvent?.Invoke();
        OnPlayerCollisionEventWithObj?.Invoke(collision.gameObject);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

    private void OnTriggerEnter(Collider other) //부딪혔을때 영향은 없고 점수가 변경된다거나? 아이템 같은거
    {
        OnPlayerTriggerEvent.Invoke();
        OnPlayerTriggerEventWithObj?.Invoke(other.gameObject);

    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
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

    public void OnDie()
    {
        life -= 1;
    }

    void FixedUpdate()
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

    public void ChangePlayerHP(int var)
    {
        hp += var;
    }
    public void PushPlayer(int pow)
    {
        //rigid.AddForce(Vector3.up * pow, ForceMode.Impulse);

    }


}
