using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    //�浹�� �̺�Ʈ�߻�: Action<GameObject> ���� invoke();
    //� ������Ʈ�� �浹�ߴ��� �˱� ����
    public static event Action OnPlayerHitSomethingEvent;
    public static event Action<GameObject> OnPlayerHitSomethingEventWithObj;
    



    public Animator anim;
    public Rigidbody rigid;
    public float moveSpeed = 5f;
    private float h;
    private float v;

    private void OnCollisionEnter(Collision collision) //�ε������� ĳ���ͳ� �浹ü�� ������ ����
    {
        OnPlayerHitSomethingEvent?.Invoke();
        OnPlayerHitSomethingEventWithObj?.Invoke(collision.gameObject);
        //� ��ü�� �΋H������ �α�â���� Ȯ���� �� �ְ�
    }

    private void OnTriggerEnter(Collider other) //�ֵ������� ������ ���� ������ ����ȴٰų�?
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
        // �̵� ���� �� �ӵ� ��� (���� ��� �� ����)
        Vector3 movement = new Vector3(h, 0, v).normalized;
        float movementSpeed = movement.magnitude;

        // �ִϸ��̼� �ӵ� ������Ʈ
        //anim.SetFloat("Speed", movementSpeed);

        // ĳ���� ȸ�� �� �̵� ó��
        if (movementSpeed > 0.1f) // �̵� �ÿ��� ȸ�� �� �̵� ó��
        {
            anim.SetBool("Walk", true);
            // ȸ�� ó�� (Slerp ����� �ε巴��)
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rigid.MoveRotation(Quaternion.Slerp(transform.rotation, newRotation, 0.2f));

            // Rigidbody�� ����� �̵� ó��
            rigid.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            anim.SetBool("Walk", false);
            // ���� �� Idle �ִϸ��̼� ���� ����
            //anim.SetTrigger("Idle");
        }
    }
}
