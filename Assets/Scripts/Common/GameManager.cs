using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public Player player;
    public InGameUI inGameUIDoc;
    public GameClearUI gameClearUIDoc;


    public int stage;

    public void Start()
    {
        gameClearUIDoc.ClearUIDeactivate();
        ChangeStage();
        ChangePlayerLife();

        Trap.OnAnyTrapCollision += TrapCollision;
        Trap.OnAnyTrapTrigger += TrapTrigger;

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnGameEnd += GameEnd;

    }
    private void GameEnd()
    {
        gameClearUIDoc.ClearUIActivate();
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

    private void playerCollisionObj(GameObject obj)
    {
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void playerTriggerObj(GameObject obj)
    {
        inGameUIDoc.UIChangePlayerHP(player.hp);

        inGameUIDoc.ChangeJumpPowerUI(4);



    }
    private void TrapCollision(Trap trap)
    {
        Debug.Log("Player Got Hit by " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void TrapTrigger(Trap trap)
    {
        Debug.Log("Player Triggered " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);

    }
}
