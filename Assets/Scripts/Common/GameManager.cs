using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    public Player player;
    public InGameUI inGameUIDoc;
    public StageClearUI stageClearUIDoc;
    public StageOverUI stageOverUIDoc;
    public CameraController cameraController;

    public Trap[] TrapList = new Trap[0];
    public int[] TrapNumList;
    
    public Transform[] pos = new Transform[0];

    public int stageNum;

    public GameObject[] stageList = new GameObject[0];

    public int totalTrapDamage;

    private GameObject trapGroup;

    public bool isPaused = false;

    public void Start()
    {
        Trap.OnAnyTrapCollision += TrapCollision;
        Trap.OnAnyTrapTrigger += TrapTrigger;

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;

        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;

        stageNum = 0;
        SetStage();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetTraps();
    }
    
    private void InitializePlayer()
    {
        player.InitializePlayer();
        ChangePlayerHP();
        ChangePlayerLife();
    }
    private void RestartStage()
    {
        inGameUIDoc.ResetTimer();
        InitializePlayer();
        GameResume();

        stageOverUIDoc.ShowGameOverUI(false);
    }
    private void ResetTraps()
    {
        InitializePlayer();

        ClearTraps();
        CreateTraps();
        ChangeStageDifficulty();
    }
    public void SetStage()
    {
        cameraController.SetCursorVisible(false);
        stageClearUIDoc.ShowClearUI(false);
        stageOverUIDoc.ShowGameOverUI(false);

        ClearTraps();
        CreateTraps();

        inGameUIDoc.ResetTimer();
        ChangeStageDifficulty();
        ChangeStageNumber();

        InitializePlayer();
    }
    private void NextStage()
    {
        stageList[stageNum].SetActive(false);
        stageNum += 1;
        stageList[stageNum].SetActive(true);
        SetStage();
        GameResume();
    }
    private void StageOver()
    {
        GamePause();
        cameraController.SetCursorVisible(true);
        stageOverUIDoc.ShowGameOverUI(true);
    }
    private void StageClear()
    {
        GamePause();
        cameraController.SetCursorVisible(true);
        stageClearUIDoc.ShowClearUI(true);
    }
    private void GamePause()
    {
        OnPaused.Invoke(true);
    }
    private void GameResume()
    {
        OnPaused.Invoke(false);
    }
    private void PlayerDie()
    {
        ChangePlayerHP();
        ChangePlayerLife();
    }

    private void CreateTraps()
    {
        totalTrapDamage = 0;
        TrapNumList = new int[TrapList.Length];
        trapGroup = new GameObject("TrapGroup");
        for (int i = 0;  i < pos.Length; i++)
        {
            float posCorrection = pos[i].gameObject.GetComponent<Collider>().bounds.size.y;
            int tNum = UnityEngine.Random.Range(0, TrapList.Length);
            Trap obj = Instantiate(TrapList[tNum], pos[i].position, Quaternion.identity);
            obj.transform.SetParent(trapGroup.transform);

            if (obj.gameObject.CompareTag("Hammer"))
            {
                posCorrection += obj.gameObject.GetComponent<Collider>().bounds.size.y;
            }

            obj.transform.Translate(Vector3.up * posCorrection);
            totalTrapDamage += obj.damage;
            TrapNumList[tNum] += 1;
        }
    }
    private void ClearTraps()
    {
        Destroy(trapGroup);
    }
    private void ChangePlayerHP()
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void ChangePlayerLife()
    {
        inGameUIDoc.ChangePlayerLifeUI(player.life);
        if(player.life <= 0)
        {
            StageOver();
        }
    }
    private void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(TrapList, TrapNumList, totalTrapDamage);
    }
    private void ChangeStageNumber()
    {
        inGameUIDoc.ChangeStageUI(stageNum);
    }
    private void playerCollisionObj(GameObject obj)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void playerTriggerObj(GameObject obj)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void TrapCollision(Trap trap)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void TrapTrigger(Trap trap)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
