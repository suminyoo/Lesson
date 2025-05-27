using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public enum Trap 
{
    spikeTrapPrefab,

}

public class GameManager : MonoBehaviour
{
    public Player player;
    public InGameUI inGameUIDoc;
    public GameClearUI gameClearUIDoc;
    public GameOverUI gameOverUIDoc;

    public GameObject spikeTrapPrefab;
    public GameObject hammerTrapPrefab;
    public GameObject poisonTrapPrefab;
    public GameObject hiddenBombTrapPrefab;

    public GameObject[] TrapList = new GameObject[4];

    [SerializeField] Transform _parent;

    public int trapNum = 5;
    public Transform[] pos = new Transform[0];

    public int stage;

    public void Start()
    {
        gameClearUIDoc.ClearUIDeactivate();
        gameOverUIDoc.GameOverUIDeactivate();

        ChangeStage();
        ChangePlayerLife();

        Trap.OnAnyTrapCollision += TrapCollision;
        Trap.OnAnyTrapTrigger += TrapTrigger;

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnGameClear += GameClear;


        CreateTrap();
    }
    private void GameClear()
    {
        gameClearUIDoc.ClearUIActivate();
    }
    private void PlayerDie()
    {
        ChangePlayerHP();
        ChangePlayerLife();
    }

    private void CreateTrap()
    {


        for (int i = 0;  i < pos.Length; i++)
        {
            GameObject obj = Instantiate(TrapList[Random.Range(0, 4)], pos[i].position, Quaternion.identity);
            obj.transform.parent = _parent;
        }


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
        if(player.life <= 0)
        {
            gameOverUIDoc.GameOverUIActivate();
        }
    }

    private void playerCollisionObj(GameObject obj)
    {
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void playerTriggerObj(GameObject obj)
    {
        inGameUIDoc.UIChangePlayerHP(player.hp);

    }
    private void TrapCollision(Trap trap)
    {
        //Debug.Log("Player Got Hit by " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void TrapTrigger(Trap trap)
    {
        //Debug.Log("Player Triggered " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);

    }
}
