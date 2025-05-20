using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    //충돌시 이벤트발생: Action<GameObject> 만들어서 invoke();
    //어떤 오브젝트에 충돌했는지 알기 위함
    public static event Action OnPlayerHitSomethingEvent;
    public static event Action<GameObject> OnPlayerHitSomethingEventWithObj;
    



    public Animator anim;
    public Rigidbody rigid;
    public float moveSpeed = 5f;
    private float h;
    private float v;

    private void OnCollisionEnter(Collision collision) //부딪혔을때 캐릭터나 충돌체에 영향이 갈때
    {
        OnPlayerHitSomethingEvent?.Invoke();
        OnPlayerHitSomethingEventWithObj?.Invoke(collision.gameObject);
        //어떤 물체와 부딫혔는지 로그창으로 확인할 수 있게
    }

    private void OnTriggerEnter(Collider other) //주딪혔을때 영향은 없고 점수가 변경된다거나?
    {
        OnPlayerHitSomethingEventWithObj?.Invoke(other.gameObject);

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }

    }
    private void FixedUpdate()
    {
        // 이동 방향 및 속도 계산 (단일 계산 후 재사용)
        Vector3 movement = new Vector3(h, 0, v).normalized;
        float movementSpeed = movement.magnitude;

        // 애니메이션 속도 업데이트
        //anim.SetFloat("Speed", movementSpeed);

        // 캐릭터 회전 및 이동 처리
        if (movementSpeed > 0.1f) // 이동 시에만 회전 및 이동 처리
        {
            anim.SetBool("Walk", true);
            // 회전 처리 (Slerp 사용해 부드럽게)
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rigid.MoveRotation(Quaternion.Slerp(transform.rotation, newRotation, 0.2f));

            // Rigidbody를 사용한 이동 처리
            rigid.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            anim.SetBool("Walk", false);
            // 멈출 때 Idle 애니메이션 상태 유지
            //anim.SetTrigger("Idle");
        }
    }
}
