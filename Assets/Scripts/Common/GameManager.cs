using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public Player player;
    public InGameUI inGameUIDoc;

    public int stage = 0;



    public void Start()
    {
        ChangeStage();
        ChangePlayerLife();

        Player.OnPlayerCollisionEvent += playerCollision;
        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;

        Player.OnPlayerTriggerEvent += playerTrigger;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
    }

    private void ChangeStage()
    {
        stage += 1;
        inGameUIDoc.ChangeStageUI(stage);

    }
    private void ChangePlayerHP(int var)
    {
        player.ChangePlayerHP(var);
        if (player.hp <= 0) 
        {
            player.OnDie();
            ChangePlayerLife();
        }
        inGameUIDoc.ChangePlayerHPUI(player.hp);

    }

    private void ChangePlayerLife()
    {
        inGameUIDoc.ChangePlayerLifeUI(player.life);

    }


    private void playerCollision()
    {
        //Debug.Log("Player Got Hit");
    }
    private void playerCollisionObj(GameObject obj)
    {
        Debug.Log("Player Got Hit by " + obj.gameObject.name);

        if (obj == null) { return; }

        if(obj.layer == 6) 
        {
            ChangePlayerHP(-10);
            if (obj.gameObject.CompareTag("Bomb"))
            {
                //플레이어 뒤로 넘어감
                player.PushPlayer(10);

            }
        }


    }
    private void playerTrigger()
    {
        Debug.Log("Player Trigger");
    }
    private void playerTriggerObj(GameObject obj)
    {
        Debug.Log("Player Triggered " + obj.gameObject.name);
        if (obj == null) { return; }

        if (obj.layer == 7)
        {
            if (obj.tag == "Life")
            {
                ChangePlayerHP(10);
                obj.SetActive(false);
            }
        }
    }
}
