using UnityEngine;
using UnityEngine.UIElements;

public class CubePlayer : MonoBehaviour
{
    //���� ���������� ���̴°� Event Action
    //���� ���� ���� ����̴� �׷��� ����. Event Bus, Event channel, Event Action ��

    [SerializeField] TestUI myTestUI;

    private float atk;

    public float ATK
    {
        get { return atk; }
        set { atk = value; }
    }


    void Start()
    {
        atk = 100;
        myTestUI.SliderValueChange(atk);

    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy collision");
            atk -= 10;
        }
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("Item collision");
            atk += 10;
        }
        myTestUI.SliderValueChange(atk);
    }


    public void changeATK(float v)
    {
        Debug.Log("v: "+v);
        atk = v;
    }

}
