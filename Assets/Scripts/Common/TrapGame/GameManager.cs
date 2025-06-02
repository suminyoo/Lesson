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
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;
        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;

        SetStage();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetTraps();
    }
    private void ResetTraps()
    {
        player.InitializePlayer();
        generateStage.ClearTrapsAndItems();
        generateStage.CreateTrapsAndItems();
        generateStage.ChangeStageDifficulty();
    }
    private void RestartStage()
    {
        inGameUIDoc.ResetTimer();
        player.InitializePlayer();
        SetStage();
        GameResume();
        stageOverUIDoc.ShowUI(false);
    }

    public void SetStage()
    {
        cameraController.SetCursorVisible(false);
        stageClearUIDoc.ShowUI(false);
        stageOverUIDoc.ShowUI(false);
        gameClearEndUI.ShowGameClearUI(false);
        playerDeadUI.ShowUI(false);

        generateStage.ClearMap();
        generateStage.GenerateChunkMap();
        generateStage.ClearTrapsAndItems();
        generateStage.CreateTrapsAndItems();
        generateStage.ChangeStageDifficulty();

        inGameUIDoc.ResetTimer();
        inGameUIDoc.ChangeStageUI(stageNum);

        player.InitializePlayer();
    }
    private void NextStage()
    {
        stageNum += 1;
        SetStage();
        GameResume();
    }
    private void PlayerDie()
    {
        GamePause();
        cameraController.SetCursorVisible(true);

        if (playerData.life <= 0) stageOverUIDoc.ShowUI(true);
        else playerDeadUI.ShowUI(true);
    }
    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        playerDeadUI.ShowUI(false);
        GameResume();
    }
    private void StageClear()
    {
        GamePause();
        cameraController.SetCursorVisible(true);

        if (stageNum +1 == clearStageNum) gameClearEndUI.ShowGameClearUI(true);
        else stageClearUIDoc.ShowUI(true);
    }
    private void GamePause() => OnPaused.Invoke(true);
    private void GameResume() => OnPaused.Invoke(false);
    public void GameQuit() => Application.Quit();
}
