using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�浹�� �̺�Ʈ�߻�: Action<GameObject> ���� invoke();
    //� ������Ʈ�� �浹�ߴ��� �˱� ����
    public static event Action OnPlayerCollisionEvent;
    public static event Action<GameObject> OnPlayerCollisionEventWithObj;

    public static event Action OnPlayerTriggerEvent;
    public static event Action<GameObject> OnPlayerTriggerEventWithObj;

    public Animator anim;
    public Rigidbody rigid;
    public Transform cameraTransform;

    public float turnSpeed = 0.05f; // �� �������� ����
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;

    public bool isGrounded = true;

    private float h;
    private float v;

    public int hp = 100;
    public int life = 3;



    private void OnCollisionEnter(Collision collision) //�ε������� ĳ���ͳ� �浹ü�� ������ ����
    {
        OnPlayerCollisionEvent?.Invoke();
        OnPlayerCollisionEventWithObj?.Invoke(collision.gameObject);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

    private void OnTriggerEnter(Collider other) //�ε������� ������ ���� ������ ����ȴٰų�? ������ ������
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
        //�÷��̾� �̵�
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude < 0.1f)
        {
            anim.SetBool("Walk", false);
            return;
        }

        // 1. ī�޶� ���� ���� ���
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 2. �̵� ������ ī�޶� �������� ��ȯ
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        // 3. ȸ�� (�ε巴��)
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
        rigid.MoveRotation(smoothedRotation);

        // 4. �̵�
        Vector3 moveAmount = moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveAmount);

        // 5. �ִϸ��̼� ó��
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
