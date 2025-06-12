using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TG_PlayerData", menuName = "Scriptable Objects/TG_PlayerData")]
public class TG_PlayerData : ScriptableObject
{

    public static event Action OnGameOverEvent;
    public static event Action OnPlayerDieEvent;

    public int stageNum = 0;


    public int hp = 100;

    public Vector3 playerRespawnPosition = new Vector3(-2, 2.5f, 0);

    public float turnSpeed = 0.05f;
    public float moveSpeed = 5f;
    public float jumpPower = 5f;

    public float maxSpeed = 8f;
    public float maxJumpPow = 8f;

    public float normalSpeed = 5f;
    public float normalJumpPow = 5f;

    public float speedRemainTime = 5f;
    public float JumpPowRemainTime = 5f;

    public bool SpeedUpUsable;
    public bool JumpPowUpUsable;

    public string DeathReason;
    public bool isDead;

    public int life;
    //{
    //    get { return life; }
    //    set { life = value;
    //        if(life <= 0) OnGameOverEvent.Invoke();
    //        else OnPlayerDieEvent.Invoke();
    //    }
    //}
}
