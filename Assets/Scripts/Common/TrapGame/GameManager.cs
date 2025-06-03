using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    [SerializeField] TG_PlayerData playerData;
    [SerializeField] Player player;
    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] StageClearUI stageClearUIDoc;
    [SerializeField] StageOverUI stageOverUIDoc;
    [SerializeField] GameClearEndUI gameClearEndUI;
    [SerializeField] PlayerDeadUI playerDeadUI;
    [SerializeField] CameraController cameraController;
    [SerializeField] GenerateStage generateStage;

    private int clearStageNum = 3;
    private int stageNum = 0;

    private void Start()
    {
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;
        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;

        SetStage();
    }
    private void Update()
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
    private void SetStage()
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
