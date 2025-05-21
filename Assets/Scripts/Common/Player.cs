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
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public bool isGrounded = true;
    private float h;
    private float v;

    public int hp = 100;
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

        //transform.Translate(Vector3.forward * moveSpeed * h * Time.deltaTime);
        //transform.Translate(Vector3.left * moveSpeed * v * Time.deltaTime);

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
    private void FixedUpdate()
    {
        // 이동 방향 및 속도 계산 (단일 계산 후 재사용)
        Vector3 movement = new Vector3(h, 0, v).normalized;
        float movementSpeed = movement.magnitude;

        if (movementSpeed > 0.1f) // 이동 시에만 회전 및 이동 처리
        {
            anim.SetBool("Walk", true);
            Quaternion newRotation = Quaternion.LookRotation(movement);// 회전 처리 (Slerp 사용해 부드럽게)
            rigid.MoveRotation(Quaternion.Slerp(transform.rotation, newRotation, 0.2f));
            rigid.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

    }

    public void ChangePlayerHP(int var)
    {
        hp += var;
    }

}
