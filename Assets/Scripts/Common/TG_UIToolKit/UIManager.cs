using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TG_PlayerData playerData;


    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] StageClearUI stageClearUIDoc;
    [SerializeField] StageOverUI stageOverUIDoc;
    [SerializeField] GameClearEndUI gameClearEndUI;
    [SerializeField] PlayerDeadUI playerDeadUI;
    [SerializeField] MainMenuUI mainMenuUI;

    private void Start()
    {
        GameManager.OnGameStateChange += UIManage;

    }
    public void UIManage(State state)
    {
        switch (state)
        {
            case State.GAME_START:
                mainMenuUI.ShowUI(false);

                break;
            case State.STAGE_RESTART:
                inGameUIDoc.ResetTimer();
                stageOverUIDoc.ShowUI(false);
                break;
            case State.STAGE_OVER:
                stageOverUIDoc.ShowUI(true);
                break;
            case State.STAGE_CLEAR:
                stageClearUIDoc.ShowUI(true);
                break;
            case State.GAME_CLEAR:
                gameClearEndUI.ShowGameClearUI(true);
                break;
            case State.PLAYER_DEAD:
                playerDeadUI.ShowUI(false);
                break;
            case State.SET_STAGE:
                stageClearUIDoc.ShowUI(false);
                stageOverUIDoc.ShowUI(false);
                gameClearEndUI.ShowGameClearUI(false);
                playerDeadUI.ShowUI(false);
                inGameUIDoc.ShowUI(true);
                inGameUIDoc.ResetTimer();
                inGameUIDoc.ChangeStageUI(playerData.stageNum); //UI binding으로 바꿔야함
                break;
        }
    }
}
