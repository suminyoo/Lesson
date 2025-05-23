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

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
    }

    private void ChangeStage()
    {
        stage += 1;
        inGameUIDoc.ChangeStageUI(stage);

    }
    private void PlayerDie()
    {
        ChangePlayerHP();
        ChangePlayerLife();
    }

    private void ChangePlayerHP()
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }

    private void ChangePlayerLife()
    {
        inGameUIDoc.ChangePlayerLifeUI(player.life);
    }

    private void playerCollisionObj(GameObject obj)
    {
        Debug.Log("Player Got Hit by " + obj.gameObject.name);
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }

    private void playerTriggerObj(GameObject obj)
    {
        Debug.Log("Player Triggered " + obj.gameObject.name);
        inGameUIDoc.ChangePlayerHPUI(player.hp);

    }
}
