using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    public TG_PlayerData playerData;

    public Player player;
    public InGameUI inGameUIDoc;
    public StageClearUI stageClearUIDoc;
    public StageOverUI stageOverUIDoc;
    public GameClearEndUI gameClearEndUI;
    public PlayerDeadUI playerDeadUI;
    public CameraController cameraController;
    public GenerateStage generateStage;

    public int clearStageNum = 3;
    public bool isPaused = false;
    public int stageNum = 0;

    public void Start()
    {
        //Trap.OnAnyTrapCollision += TrapCollisionTrigger;
        //Trap.OnAnyTrapTrigger += TrapCollisionTrigger;

        //Player.OnPlayerCollisionEventWithObj += playerCollisionTriggerObj;
        //Player.OnPlayerTriggerEventWithObj += playerCollisionTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;

        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;

        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;

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

    private void InitializePlayer()
    {
        player.InitializePlayer();
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
        inGameUIDoc.ChangeStageUI(stageNum);

        InitializePlayer();
    }
    private void NextStage()
    {
        stageNum += 1;
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

        if (playerData.life <= 0) StageOver();
        else playerDeadUI.ShowPlayerDeadUI(true);

    }
    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        if (playerData.life <= 0)
        {
            StageOver();
        }
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
    private void GamePause() => OnPaused.Invoke(true);
    private void GameResume() => OnPaused.Invoke(false);
    public void GameQuit()
    {
        Application.Quit();
    }
}
