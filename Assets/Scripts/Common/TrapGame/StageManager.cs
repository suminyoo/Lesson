using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GenerateStage generateStage;
    [SerializeField] private TG_PlayerData playerData;

    public StageDataSO[] stages;
    public int currentStageIndex = 0;
    private int currentRepeat = 0;

    public void LoadCurrentStage()
    {
        StageDataSO data = stages[currentStageIndex];

        int trapCount = data.trapCount + data.trapIncrementPerRepeat * currentRepeat;
        int itemCount = data.itemCount + data.itemIncrementPerRepeat * currentRepeat;

        generateStage.SetTileBehaviorChances(data.movingTileChance, data.fallingTileChance);
        generateStage.SetTrapList(data.trapPrefabs, trapCount);
        generateStage.SetItemList(data.itemPrefabs, itemCount);
        generateStage.SetChunkList(data.chunkPrefabs, data.chunkCount);
        generateStage.ResetStage();
    }

    public void NextStage()
    {
        StageDataSO data = stages[currentStageIndex];

        playerData.stage++;
        currentRepeat++;

        if (currentRepeat < data.repeatCountPerStage)
        {
            LoadCurrentStage();
        }
        else
        {
            currentRepeat = 0;
            currentStageIndex++;
            LoadCurrentStage();
        }
    }

    public void RestartStage()
    {
        LoadCurrentStage();
    }

    public bool IsFinalStage()
    {
        bool isLastStageIndex = currentStageIndex == stages.Length - 1;

        bool isLastRepeat = currentRepeat == stages[currentStageIndex].repeatCountPerStage - 1;

        return isLastStageIndex && isLastRepeat;
    }
    public void ResetProgress()
    {
        playerData.stage = 1;
        currentStageIndex = 0;
        PlayerPrefs.DeleteKey("CurrentStage"); // 저장한 키 이름에 따라 바꿔줘
    }
}
