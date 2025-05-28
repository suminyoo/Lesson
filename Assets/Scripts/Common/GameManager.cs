using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Player player;
    public InGameUI inGameUIDoc;
    public GameClearUI gameClearUIDoc;
    public GameOverUI gameOverUIDoc;

    public Trap spikeTrapPrefab;
    public Trap hammerTrapPrefab;
    public Trap poisonTrapPrefab;
    public Trap hiddenBombTrapPrefab;


    public static int trapNum = 4;
    public Trap[] TrapList = new Trap[trapNum];
    public int[] TrapNumList = new int[trapNum];

    [SerializeField] Transform _parent;

    
    public Transform[] pos = new Transform[0];

    public int stage;
    public int totalTrapDamage;

    public void Start()
    {
        gameClearUIDoc.ClearUIDeactivate();
        gameOverUIDoc.GameOverUIDeactivate();

        Trap.OnAnyTrapCollision += TrapCollision;
        Trap.OnAnyTrapTrigger += TrapTrigger;

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnGameClear += GameClear;


        CreateTrap();


        ChangeStage();
        ChangePlayerLife();
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
            float posCorrec = pos[i].gameObject.GetComponent<Collider>().bounds.size.y;
            int tnum = Random.Range(0, trapNum);
            Trap obj = Instantiate(TrapList[tnum], pos[i].position, Quaternion.identity);
            obj.transform.parent = _parent;

            if (obj.gameObject.CompareTag("Hammer"))
            {
                posCorrec += obj.gameObject.GetComponent<Collider>().bounds.size.y;
            }

            obj.transform.Translate(Vector3.up * posCorrec);
            totalTrapDamage += obj.damage;
            TrapNumList[tnum] += 1;
        }

    }



    private void ChangeStage()
    {
        stage += 1;
        inGameUIDoc.UIChangeStage(stage);
        inGameUIDoc.ChangeDifficultyUI(TrapNumList, totalTrapDamage);
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
