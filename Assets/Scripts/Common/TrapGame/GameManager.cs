using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
//높이에 따른 청크 생성
//날아오는 함정
//이펙트
//traps
public enum State
{
    GAME_START,
    STAGE_OVER,
    STAGE_RESTART,
    STAGE_CLEAR,
    GAME_CLEAR,
    SET_STAGE,
    PLAYER_DEAD,
}
public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    public static event Action<State> OnGameStateChange;


    [SerializeField] TG_PlayerData playerData;
    [SerializeField] Player player;

    [SerializeField] CameraController cameraController;
    [SerializeField] GenerateStage generateStage;

    private int clearStageNum = 3;

    private void Start()
    {
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;
        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;
        MainMenuUI.OnGameStart += StartGame;

        Trap.OnAnyTrapCollision += DeathReason;
        Trap.OnAnyTrapTrigger += DeathReason;

        StartCoroutine(LateStart());

    }
    void DeathReason(string str)
    {
        //if (str == null) playerData.DeathReason = "Killed by";
        playerData.DeathReason = "Killed By " + str;
    }
    IEnumerator LateStart()
    {
        yield return null;
        MainMenu();
    }
    private void MainMenu()
    {
        GamePause();
    }
    private void StartGame()
    {
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
        player.InitializePlayer();
        SetStage();
        GameResume();
        OnGameStateChange.Invoke(State.STAGE_RESTART);

    }
    private void SetStage()
    {
        cameraController.SetCursorVisible(false);

        generateStage.ClearMap();
        generateStage.GenerateChunkMap();
        generateStage.ClearTrapsAndItems();
        generateStage.CreateTrapsAndItems();
        generateStage.ChangeStageDifficulty();

        OnGameStateChange.Invoke(State.SET_STAGE);

        player.InitializePlayer();
    }
    private void NextStage()
    {
        playerData.stageNum += 1;
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
            OnGameStateChange.Invoke(State.STAGE_OVER);

        }
        else OnGameStateChange.Invoke(State.PLAYER_DEAD);
    }
    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        OnGameStateChange.Invoke(State.PLAYER_DEAD);
        GameResume();
    }
    private void StageClear()
    {
        SoundManager.Instance.PlaySFX(ESfx.StageClear);
        GamePause();

        if (playerData.stageNum +1 == clearStageNum) OnGameStateChange.Invoke(State.GAME_CLEAR);
        else OnGameStateChange.Invoke(State.STAGE_CLEAR);
        
    }
    private void GamePause() => OnPaused.Invoke(true);
    private void GameResume() => OnPaused.Invoke(false);
    public void GameQuit() => Application.Quit();
}
