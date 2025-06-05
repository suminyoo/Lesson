using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
//높이에 따른 청크 생성
//날아오는 함정
//이펙트
//traps
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
    [SerializeField] MainMenuUI mainMenuUI;

    private int clearStageNum = 3;
    private int stageNum = 0;

    private void Start()
    {
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;
        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;
        MainMenuUI.OnGameStart += StartStage;

        Trap.OnAnyTrapCollision += DeathReason;
        Trap.OnAnyTrapTrigger += DeathReason;

        StartCoroutine(LateStart());

    }
    void DeathReason(string str)
    {
        if (str == null) playerData.DeathReason = "Killed by";
        playerData.DeathReason = "Killed By " + str;
    }
    IEnumerator LateStart()
    {
        yield return null;
        MainMenu();
    }
    private void MainMenu()
    {
        mainMenuUI.ShowUI(true);
        GamePause();
    }

    private void StartStage()
    {
        mainMenuUI.ShowUI(false);
        GameResume();
        SetStage();
        SoundManager.Instance.PlayBGM(EBgm.BGM_GAME);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetTraps();
        if (Input.GetKeyDown(KeyCode.T)) ResetMap();

    }
    private void ResetTraps()
    {
        player.InitializePlayer();
        generateStage.ClearTrapsAndItems();
        generateStage.CreateTrapsAndItems();
        generateStage.ChangeStageDifficulty();
    }
    private void ResetMap()
    {
        player.InitializePlayer();
        generateStage.ClearTrapsAndItems();
        generateStage.CreateTrapsAndItems();
        generateStage.ClearMap();
        generateStage.GenerateChunkMap();
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
        inGameUIDoc.ShowUI(true);

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
        SoundManager.Instance.PlaySFX(ESfx.Death);
        GamePause();

        if (playerData.life <= 0)
        {
            SoundManager.Instance.PlaySFX(ESfx.StageOver);
            stageOverUIDoc.ShowUI(true);
        }
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
        SoundManager.Instance.PlaySFX(ESfx.StageClear);

        GamePause();

        if (stageNum +1 == clearStageNum) gameClearEndUI.ShowGameClearUI(true);
        else stageClearUIDoc.ShowUI(true);
    }
    private void GamePause() => OnPaused.Invoke(true);
    private void GameResume() => OnPaused.Invoke(false);
    public void GameQuit() => Application.Quit();
}
