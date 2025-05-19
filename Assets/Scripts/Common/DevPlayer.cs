using UnityEngine;

public class DevPlayer : MonoBehaviour
{

    public Animator anim;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            anim.SetTrigger("Fall");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            anim.SetTrigger("Hit");
        }
    }
}
