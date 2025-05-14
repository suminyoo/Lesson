using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int enemyHealth = 1000;

    void myEnemy()
    {
        Debug.Log("My Enemy");
    }
    public int EnemyHealth(int hp)
    {
        hp -= 10;
        return hp;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected!");
        enemyHealth = EnemyHealth(enemyHealth);

        EventBus<PlayerEvent>.Raise(new PlayerEvent
        {
            health = EnemyHealth(enemyHealth),
            mana = 20,
            myAction = myEnemy,
            myFunc = EnemyHealth,

        });

    }

    private void Start()
    {

    }


}

