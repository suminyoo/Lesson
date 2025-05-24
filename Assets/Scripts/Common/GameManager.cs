using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public Player player;
    public InGameUI inGameUIDoc;

    public int stage;

    public void Start()
    {
        ChangeStage();
        ChangePlayerLife();

        Trap.OnAnyTrapCollision += playerCollisionTrap;
        Trap.OnAnyTrapTrigger += playerTriggerTrap;

        //Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        //Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;

    }
    private void PlayerDie()
    {
        ChangePlayerHP();
        ChangePlayerLife();
    }

    private void ChangeStage()
    {
        stage += 1;
        inGameUIDoc.UIChangeStage(stage);

    }

    private void ChangePlayerHP()
    {
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void ChangePlayerLife()
    {
        inGameUIDoc.UIChangePlayerLife(player.life);
    }

    //private void playerCollisionObj(Player player, GameObject obj)
    //{
    //    //Debug.Log("Player Got Hit by " + obj.gameObject.name);
    //    inGameUIDoc.UIChangePlayerHP(player.hp);
    //}

    //private void playerTriggerObj(Player player, GameObject obj)
    //{
    //    //Debug.Log("Player Triggered " + obj.gameObject.name);
    //    inGameUIDoc.UIChangePlayerHP(player.hp);

    //}

    private void playerCollisionTrap(Player player, Trap trap)
    {
        Debug.Log("Player Got Hit by " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void playerTriggerTrap(Player player, Trap trap)
    {
        Debug.Log("Player Triggered " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);

    }
}
