using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] StageClearUI stageClearUIDoc;
    [SerializeField] StageOverUI stageOverUIDoc;
    [SerializeField] GameClearEndUI gameClearEndUI;
    [SerializeField] PlayerDeadUI playerDeadUI;
    [SerializeField] MainMenuUI mainMenuUI;

    private void Start()
    {
        GameManager.OnGameStateChange += ManageUI;
    }
    public void ManageUI(State state)
    {
        switch (state)
        {
            case State.GAME_START:
                mainMenuUI.ShowUI(false);
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
                playerDeadUI.ShowUI(true);
                break;

            case State.PLAYER_RESPAWN:
                playerDeadUI.ShowUI(false);
                break;

            case State.SET_STAGE:
                stageClearUIDoc.ShowUI(false);
                stageOverUIDoc.ShowUI(false);
                gameClearEndUI.ShowGameClearUI(false);
                playerDeadUI.ShowUI(false);
                inGameUIDoc.ShowUI(true);
                inGameUIDoc.ResetTimer();
                break;

        }
    }
}
