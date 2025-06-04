using UnityEngine;

public class FallingGround : Trap
{

    //높이에 따른 청크 생성
    //앞에서 날아오는 함정(동적요소추가
    //옆에서 날아오는 함정(동적요소추가
    //이펙트매니저
    //더많은 함정
    public void Awake()
    {
        base.damage = 0;
    }

    protected override void OnCollisionEnter(Collision collision)
    {

        ShakeGround();
        base.OnCollisionEnter(collision);
    }

    public void ShakeGround()
    {

    }

    public void Fall()
    {

    }
}
