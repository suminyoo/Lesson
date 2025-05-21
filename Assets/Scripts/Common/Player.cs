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
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public bool isGrounded = true;
    private float h;
    private float v;

    public int hp = 100;
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
        // �̵� ���� �� �ӵ� ��� (���� ��� �� ����)
        Vector3 movement = new Vector3(h, 0, v).normalized;
        float movementSpeed = movement.magnitude;

        if (movementSpeed > 0.1f) // �̵� �ÿ��� ȸ�� �� �̵� ó��
        {
            anim.SetBool("Walk", true);
            Quaternion newRotation = Quaternion.LookRotation(movement);// ȸ�� ó�� (Slerp ����� �ε巴��)
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
