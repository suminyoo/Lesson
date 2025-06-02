using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    public Player player;
    public InGameUI inGameUIDoc;
    public StageClearUI stageClearUIDoc;
    public StageOverUI stageOverUIDoc;
    public GameClearEndUI gameClearEndUI;
    public PlayerDeadUI playerDeadUI;
    public CameraController cameraController;
    public GenerateStage generateStage;

    //public GameObject[] stageList = new GameObject[0];

    public int clearStageNum = 3;
    public bool isPaused = false;
    public int stageNum = 0;

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

        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;

        //DeactivateAllStage();
        //stageList[stageNum].SetActive(true); //first stage
        SetStage();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetTraps();
    }
    private void ResetTraps()
    {
        InitializePlayer();
        generateStage.ClearTraps();
        generateStage.CreateTraps();
        generateStage.ChangeStageDifficulty();
    }
    //private void DeactivateAllStage()
    //{
    //    for (int i = 0; i < stageList.Length; i++)
    //    {
    //        stageList[i].SetActive(false);
    //    }
    //}
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
        SetStage();
        GameResume();
        stageOverUIDoc.ShowStageOverUI(false);
    }

    public void SetStage()
    {
        cameraController.SetCursorVisible(false);
        stageClearUIDoc.ShowClearUI(false);
        stageOverUIDoc.ShowStageOverUI(false);
        gameClearEndUI.ShowGameClearUI(false);
        playerDeadUI.ShowPlayerDeadUI(false);

        generateStage.ClearMap();
        generateStage.GenerateChunkMap();
        generateStage.ClearTraps();
        generateStage.CreateTraps();
        generateStage.ChangeStageDifficulty();

        inGameUIDoc.ResetTimer();
        ChangeStageNumber();

        InitializePlayer();
    }
    private void NextStage()
    {
        //stageList[stageNum].SetActive(false);
        stageNum += 1;
        //stageList[stageNum].SetActive(true);
        SetStage();
        GameResume();
    }
    private void GameClearEnd()
    {
        GamePause();
        cameraController.SetCursorVisible(true);
        gameClearEndUI.ShowGameClearUI(true);
    }
    private void StageOver()
    {
        GamePause();
        cameraController.SetCursorVisible(true);
        stageOverUIDoc.ShowStageOverUI(true);
    }
    private void PlayerDie()
    {
        GamePause();
        cameraController.SetCursorVisible(true);

        if (player.life <= 0) StageOver();
        else playerDeadUI.ShowPlayerDeadUI(true);

    }
    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        ChangePlayerHP();
        ChangePlayerLife();
        playerDeadUI.ShowPlayerDeadUI(false);
        GameResume();
    }
    private void StageClear()
    {
        GamePause();
        cameraController.SetCursorVisible(true);

        if (stageNum == clearStageNum)
        {
            GameClearEnd();
            return;
        }

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

    private void ChangePlayerHP()
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void ChangePlayerLife()
    {
        inGameUIDoc.ChangePlayerLifeUI(player.life);
        playerDeadUI.ChangePlayerRemainLifeUI(player.life);
        if (player.life <= 0)
        {
            StageOver();
        }
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
