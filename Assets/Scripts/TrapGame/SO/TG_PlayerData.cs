using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TG_PlayerData", menuName = "Scriptable Objects/TG_PlayerData")]
public class TG_PlayerData : ScriptableObject
{
    public int stage = 1;
    public int score = 0;
    public string DeathReason;
    public bool isDead;
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


    #region HpLife

    [SerializeField] private int _hp = 100;
    [SerializeField] private int _life = 3;

    public event Action OnLifeLost;
    public event Action OnStageOver;

    public int maxHP = 100;
    public int hp => _hp;
    public int life => _life;

    public void Reset()
    {
        _hp = maxHP;
        _life = 3;
    }
    public void AddLife(int amount)
    {
        _life += amount;
    }
    public void ChangeHP(int amount)
    {
        _hp = Mathf.Clamp(_hp + amount, 0, maxHP);

        if (_hp <= 0)
        {
            _life--;
            if (_life <= 0) OnStageOver?.Invoke();
            else OnLifeLost?.Invoke(); 
        }
    }
    public void Respawn()
    {
        _hp = maxHP;
    }

    #endregion
}
