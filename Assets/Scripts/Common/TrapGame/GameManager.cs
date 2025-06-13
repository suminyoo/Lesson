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
    STAGE_CLEAR,
    GAME_CLEAR,
    SET_STAGE,
    PLAYER_DEAD,
    PLAYER_RESPAWN,
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
        StageOverUI.OnRestartStageEvent += SetStage;
        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;
        MainMenuUI.OnGameStart += StartGame;
        Trap.OnAnyTrapCollision += DeathReason;
        Trap.OnAnyTrapTrigger += DeathReason;

        StartCoroutine(LateStart());
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
        generateStage.ResetStage();
        OnGameStateChange.Invoke(State.SET_STAGE);
        player.InitializePlayer();
        GameResume();
    }
    private void NextStage()
    {
        playerData.stage += 1;
        SetStage();
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
        OnGameStateChange.Invoke(State.PLAYER_RESPAWN);
        GameResume();
    }
    private void StageClear()
    {
        SoundManager.Instance.PlaySFX(ESfx.StageClear);
        GamePause();

        if (playerData.stage +1 == clearStageNum) 
            OnGameStateChange.Invoke(State.GAME_CLEAR);
        else 
            OnGameStateChange.Invoke(State.STAGE_CLEAR);
    }
    private void GamePause()
    {
        //Time.timeScale = 0f;
        OnPaused.Invoke(true);
    }
    private void GameResume() 
    {
        //Time.timeScale = 1f;
        OnPaused.Invoke(false);
    }
    public void GameQuit() => Application.Quit();
}
