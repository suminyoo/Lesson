using System;
using System.Collections;
using UnityEngine;

public enum State
{
    GAME_START,
    STAGE_OVER,
    STAGE_CLEAR,
    GAME_CLEAR,
    SET_STAGE,
    PLAYER_DEAD,
    PLAYER_RESPAWN,

    GAME_PAUSE,
    GAME_RESUME,
    OPTION,
    MAIN_MENU,
}
public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;
    public static event Action<State> OnGameStateChange;

    [SerializeField] TG_PlayerData playerData;
    [SerializeField] Player player;

    [SerializeField] CameraController cameraController;
    [SerializeField] GenerateStage generateStage;
    [SerializeField] StageManager stageManager;

    private void Start()
    {
        player.OnStageClear += HandleStageClear;

        playerData.OnLifeLost += HandleLifeLost;
        playerData.OnStageOver += HandleStageOver;

        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += SetStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;
        MainMenuUI.OnGameStart += StartGame;
        Trap.OnAnyTrapCollision += DeathReason;
        Trap.OnAnyTrapTrigger += DeathReason;

        StartCoroutine(LateStart());
        playerData.stage = 1;
    }
    private void HandleGameClear()
    {
        GamePause();
        SoundManager.Instance.PlaySFX(ESfx.StageClear); //to game clear sound
        OnGameStateChange?.Invoke(State.GAME_CLEAR);
    }
    private void HandleStageClear()
    {
        if (stageManager.IsFinalStage())
        {
            HandleGameClear();
        }
        else
        {
            GamePause();
            SoundManager.Instance.PlaySFX(ESfx.StageClear);
            OnGameStateChange?.Invoke(State.STAGE_CLEAR);
        }

    }
    private void HandleLifeLost()
    {
        GamePause();
        SoundManager.Instance.PlaySFX(ESfx.Death);
        OnGameStateChange?.Invoke(State.PLAYER_DEAD);
    }

    private void HandleStageOver()
    {
        GamePause();
        SoundManager.Instance.PlaySFX(ESfx.StageOver);
        OnGameStateChange?.Invoke(State.STAGE_OVER);
    }

    void DeathReason(string str)
    {
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
        OnGameStateChange.Invoke(State.GAME_START);
        SetStage();
        SoundManager.Instance.PlayBGM(EBgm.BGM_GAME);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T)) SetStage();
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.InitializePlayer();
            generateStage.ResetTrapsAndItems();
        }
    }
   
    private void SetStage()
    {
        stageManager.LoadCurrentStage();
        OnGameStateChange.Invoke(State.SET_STAGE);
        player.InitializePlayer();
        GameResume();
    }
    private void NextStage()
    {
        stageManager.NextStage();
        OnGameStateChange.Invoke(State.SET_STAGE);
        player.InitializePlayer();
        GameResume();
    }

    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        OnGameStateChange.Invoke(State.PLAYER_RESPAWN);
        GameResume();
    }

    private void GamePause() => OnPaused.Invoke(true);
    private void GameResume() => OnPaused.Invoke(false);
    public void GameQuit() => Application.Quit();
}
