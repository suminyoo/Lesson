using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnPaused;

    public Player player;
    public InGameUI inGameUIDoc;
    public StageClearUI stageClearUIDoc;
    public StageOverUI stageOverUIDoc;
    public GameClearEndUI gameClearEndUI;
    public PlayerDeadUI playerDeadUI;
    public CameraController cameraController;

    public GameObject[] stageList = new GameObject[0];
    public Transform[] trapPositions = new Transform[0];
    public Trap[] trapPrefabList = new Trap[0];
    private GameObject trapGroup;
    private GameObject tileGroup;

    //public GameObject baseTilePrefab;
    //public int xTileCount = 40;       // x축 방향 타일 개수 (0~40, 2 간격)
    //public int tileSize = 2;          // 타일 간격
    //private int[] zOffsets = new int[] {-4, -2, 0, 2, 4 };

    public GameObject normalTilePrefab;
    public GameObject slopeTilePrefab;
    public GameObject secondFloorTilePrefab;

    //public GameObject baseTilePrefab; // 2x2 크기의 기본 타일 프리팹
    //public int xTileCount = 21;       // x축 타일 개수 (0 ~ 40)
    //public int tileSize = 2;          // 타일 간격
    //public int minZ = -4;
    //public int maxZ = 4;

    //private List<int> zOffsets;

    public int xTileCount = 21;       // 0~40까지 2 간격
    public int tileSize = 2;
    private int[] zOffsets = new int[] { -4, -2, 0, 2, 4 };
    private HashSet<Vector2> firstFloorPositions = new HashSet<Vector2>();



    public int[] spawnedTrapNums;
    public int totalTrapDamage;

    public bool isPaused = false;
    public int stageNum = 0;

    public void Start()
    {
        Trap.OnAnyTrapCollision += TrapCollision;
        Trap.OnAnyTrapTrigger += TrapTrigger;

        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
        Player.OnPlayerDie += PlayerDie;
        Player.OnStageClear += StageClear;

        StageClearUI.OnNextStageEvent += NextStage;
        StageOverUI.OnRestartStageEvent += RestartStage;

        PlayerDeadUI.OnPlayerRespawnEvent += PlayerRespawn;

        DeactivateAllStage();
        //stageList[stageNum].SetActive(true); //first stage
        //SetStage();

        // zOffsets 자동 생성 (-4 ~ 4, 2 간격)
        //zOffsets = new List<int>();
        //for (int z = minZ; z <= maxZ; z += 2)
        //    zOffsets.Add(z);

        cameraController.SetCursorVisible(false);
        stageClearUIDoc.ShowClearUI(false);
        stageOverUIDoc.ShowStageOverUI(false);
        gameClearEndUI.ShowGameClearUI(false);
        playerDeadUI.ShowPlayerDeadUI(false);
        GenerateMap();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetTraps();
    }
    private void ResetTraps()
    {
        InitializePlayer();
        ClearTraps();
        CreateTraps();
        ChangeStageDifficulty();
    }
    private void DeactivateAllStage()
    {
        for (int i = 0; i < stageList.Length; i++)
        {
            stageList[i].SetActive(false);
        }
    }
    private void InitializePlayer()
    {
        player.InitializePlayer();
        ChangePlayerHP();
        ChangePlayerLife();
    }
    private void RestartStage()
    {
        inGameUIDoc.ResetTimer();
        InitializePlayer();

        SetStage();

        GameResume();
        stageOverUIDoc.ShowStageOverUI(false);
    }

    List<int> usedXIndices = new List<int>(); // 2층 구간 시작 xIndex 보관

    void GenerateMap()
    {
        firstFloorPositions.Clear();
        usedXIndices.Clear();

        // 1층 생성
        for (int xIndex = 0; xIndex < xTileCount; xIndex++)
        {
            float xPos = xIndex * tileSize;

            foreach (int zOffset in zOffsets)
            {
                if (UnityEngine.Random.value > 0.6f) continue;

                Vector3 pos = new Vector3(xPos, 0f, zOffset);
                Instantiate(normalTilePrefab, pos, Quaternion.identity, transform);
                firstFloorPositions.Add(new Vector2(xPos, zOffset));
            }
        }

        // 2층 구간 생성
        int regionCount = UnityEngine.Random.Range(1, 4);
        int maxAttempts = 10;
        int attempts = 0;

        while (usedXIndices.Count < regionCount && attempts < maxAttempts)
        {
            if (SpawnSecondFloorRegion()) attempts++;
            else break;
        }
    }
    bool SpawnSecondFloorRegion()
    {
        int[,] sizeOptions = new int[,] { { 2, 2 }, { 2, 3 }, { 3, 2 } };
        int optionIndex = UnityEngine.Random.Range(0, sizeOptions.GetLength(0));
        int width = sizeOptions[optionIndex, 0];
        int depth = sizeOptions[optionIndex, 1];

        int minXStartIndex = 3;
        int maxXStart = xTileCount - width - 1;
        if (maxXStart < minXStartIndex) return false;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            int xStartIndex = UnityEngine.Random.Range(minXStartIndex, maxXStart + 1);

            bool tooClose = usedXIndices.Any(i => Mathf.Abs(i - xStartIndex) < 3);
            if (tooClose) continue;

            float xStart = xStartIndex * tileSize;

            List<int> zCandidates = new List<int>(zOffsets);
            zCandidates.RemoveAll(z => z + (depth - 1) * tileSize > 4);
            if (zCandidates.Count == 0) return false;

            int zStart = zCandidates[UnityEngine.Random.Range(0, zCandidates.Count)];

            // 2층 및 1층 생성
            for (int dx = 0; dx < width; dx++)
            {
                for (int dz = 0; dz < depth; dz++)
                {
                    float x = xStart + dx * tileSize;
                    float z = zStart + dz * tileSize;
                    Vector2 pos2D = new Vector2(x, z);

                    if (!firstFloorPositions.Contains(pos2D))
                    {
                        Instantiate(normalTilePrefab, new Vector3(x, 0f, z), Quaternion.identity, transform);
                        firstFloorPositions.Add(pos2D);
                    }

                    Instantiate(secondFloorTilePrefab, new Vector3(x, 2f, z), Quaternion.identity, transform);
                }
            }

            // 경사 타일 위치
            float slopeX = xStart - tileSize;

            // 경사 타일 밑과 앞의 1층 타일 위치
            // 이 부분이 문제였을 수 있음. 무조건 1층 타일 깔아주기
            for (int dz = 0; dz < depth; dz++)
            {
                float z = zStart + dz * tileSize;
                Vector2 below = new Vector2(slopeX, z);
                Vector2 front = new Vector2(slopeX - tileSize, z);

                if (!firstFloorPositions.Contains(below))
                {
                    Instantiate(normalTilePrefab, new Vector3(slopeX, 0f, z), Quaternion.identity, transform);
                    firstFloorPositions.Add(below);
                }
                if (!firstFloorPositions.Contains(front))
                {
                    Instantiate(normalTilePrefab, new Vector3(slopeX - tileSize, 0f, z), Quaternion.identity, transform);
                    firstFloorPositions.Add(front);
                }
            }

            // 가능한 z 위치 중에서 랜덤 선택해 경사 타일 생성
            List<float> possibleZ = new List<float>();
            for (int dz = 0; dz < depth; dz++)
            {
                float z = zStart + dz * tileSize;
                Vector2 below = new Vector2(slopeX, z);
                Vector2 front = new Vector2(slopeX - tileSize, z);
                if (firstFloorPositions.Contains(below) && firstFloorPositions.Contains(front))
                {
                    possibleZ.Add(z);
                }
            }

            if (possibleZ.Count > 0)
            {
                float slopeZ = possibleZ[UnityEngine.Random.Range(0, possibleZ.Count)];
                Vector3 slopePos = new Vector3(slopeX, 2f, slopeZ);
                Instantiate(slopeTilePrefab, slopePos, Quaternion.Euler(0f, 180f, 0f), transform);
            }
            else
            {
                // 경사 타일 생성 조건 불충분 시 로깅 또는 디버그용 메시지 출력 가능
                Debug.LogWarning("경사 타일 생성 가능한 위치 없음");
            }

            usedXIndices.Add(xStartIndex);
            return true;
        }
        return false;
    }

    //private void GenerateMap()
    //{
    //    tileGroup = new GameObject("TileGroup");
    //    for (int xIndex = 0; xIndex < xTileCount; xIndex++)
    //    {
    //        float xPos = xIndex * tileSize;

    //        // 각 z 위치마다 랜덤하게 타일 생성 여부 결정
    //        foreach (int zOffset in zOffsets)
    //        {
    //            if (UnityEngine.Random.value < 0.5f) // 50% 확률로 타일 생성
    //                continue;

    //            Vector3 tilePos = new Vector3(xPos, 0f, zOffset);
    //            GameObject tile = Instantiate(baseTilePrefab, tilePos, Quaternion.identity, transform);
    //            tile.transform.SetParent(tileGroup.transform);

    //        }
    //    }
    //}

    public void SetStage()
    {
        cameraController.SetCursorVisible(false);
        stageClearUIDoc.ShowClearUI(false);
        stageOverUIDoc.ShowStageOverUI(false);
        gameClearEndUI.ShowGameClearUI(false);
        playerDeadUI.ShowPlayerDeadUI(false);

        ClearTraps();
        CreateTraps();

        inGameUIDoc.ResetTimer();
        ChangeStageDifficulty();
        ChangeStageNumber();

        InitializePlayer();
    }
    private void NextStage()
    {
        stageList[stageNum].SetActive(false);
        stageNum += 1;
        stageList[stageNum].SetActive(true);
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

        if (player.life <= 0) StageOver();
        else playerDeadUI.ShowPlayerDeadUI(true);

    }
    private void PlayerRespawn()
    {
        player.RespawnPlayer();
        ChangePlayerHP();
        ChangePlayerLife();
        playerDeadUI.ShowPlayerDeadUI(false);
        GameResume();
    }
    private void StageClear()
    {
        GamePause();
        cameraController.SetCursorVisible(true);

        if (stageNum + 1 == stageList.Length)
        {
            GameClearEnd();
            return;
        }

        stageClearUIDoc.ShowClearUI(true);
    }
    private void GamePause()
    {
        OnPaused.Invoke(true);
    }
    private void GameResume()
    {
        OnPaused.Invoke(false);
    }
    private void CreateTraps()
    {
        totalTrapDamage = 0;
        spawnedTrapNums = new int[trapPrefabList.Length];
        trapGroup = new GameObject("TrapGroup");

        for (int i = 0;  i < trapPositions.Length; i++)
        {
            float posCorrection = trapPositions[i].gameObject.GetComponent<Collider>().bounds.size.y; //ground y size
            int tNum = UnityEngine.Random.Range(0, trapPrefabList.Length);

            Trap trapInstance = Instantiate(trapPrefabList[tNum], trapPositions[i].position, Quaternion.identity);
            Transform placementAnchor = trapInstance.transform.Find("PlacementAnchor"); //PlacementAnchor

            Vector3 offset = trapInstance.transform.position - placementAnchor.position;

            trapInstance.transform.position = trapPositions[i].position + offset; //trap to PlacementAnchor position

            trapInstance.transform.SetParent(trapGroup.transform);
            trapInstance.transform.Translate(Vector3.up * posCorrection); // add ground y size

            totalTrapDamage += trapInstance.damage;
            spawnedTrapNums[tNum] += 1;
        }
    }
    private void ClearTraps()
    {
        Destroy(trapGroup);
    }
    private void ChangePlayerHP()
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void ChangePlayerLife()
    {
        inGameUIDoc.ChangePlayerLifeUI(player.life);
        playerDeadUI.ChangePlayerRemainLifeUI(player.life);
        if (player.life <= 0)
        {
            StageOver();
        }
    }
    private void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(trapPrefabList, spawnedTrapNums, totalTrapDamage);
    }
    private void ChangeStageNumber()
    {
        inGameUIDoc.ChangeStageUI(stageNum);
    }
    private void playerCollisionObj(GameObject obj)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void playerTriggerObj(GameObject obj)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void TrapCollision(Trap trap)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    private void TrapTrigger(Trap trap)
    {
        inGameUIDoc.ChangePlayerHPUI(player.hp);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
