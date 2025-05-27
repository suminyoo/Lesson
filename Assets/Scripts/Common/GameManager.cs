using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


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

    public static int trapNum = 4;
    public GameObject[] TrapList = new GameObject[trapNum];

    [SerializeField] Transform _parent;

    
    public Transform[] pos = new Transform[0];

    public int stage;

    public void Start()
    {
        gameClearUIDoc.ClearUIDeactivate();
        gameOverUIDoc.GameOverUIDeactivate();

        ChangeStage();
        ChangePlayerLife();

        Traps.OnAnyTrapCollision += TrapCollision;
        Traps.OnAnyTrapTrigger += TrapTrigger;

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

        TrapList[0] = spikeTrapPrefab;
        TrapList[1] = hammerTrapPrefab;
        TrapList[2] = poisonTrapPrefab;
        TrapList[3] = hiddenBombTrapPrefab;

        
        for (int i = 0;  i < pos.Length; i++)
        {
            int posCorrec = 2;
            GameObject obj = Instantiate(TrapList[Random.Range(0, trapNum)], pos[i].position, Quaternion.identity);
            obj.transform.parent = _parent;
            if (obj.gameObject.CompareTag("Hammer"))
            {
                posCorrec = 6;
            }
            obj.transform.Translate(Vector3.up * posCorrec);
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
    private void TrapCollision(Traps trap)
    {
        //Debug.Log("Player Got Hit by " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);
    }

    private void TrapTrigger(Traps trap)
    {
        //Debug.Log("Player Triggered " + trap.name);
        inGameUIDoc.UIChangePlayerHP(player.hp);

    }
}
