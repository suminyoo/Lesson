using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] GameObject finishPrefab;
    [SerializeField] GameObject lifeItemPrefab;

    private Trap[] trapPrefabList = new Trap[0];
    private Item[] itemPrefabList = new Item[0];
    private GameObject[] chunkPrefabList = new GameObject[0];

    private int desiredTrapCount;
    private int desiredItemCount;
    private int desiredchunkCount;

    public int[] spawnedTrapNums; // 생성된 함정 개수 배열
    public int totalTrapDamage;   // 함정 총 피해량

    private int chunkDepth = 4;
    private float tileSize = 2f;

    private GameObject trapGroup;
    private GameObject chunkGroup;
    private GameObject itemGroup;

    private List<Vector3> trapCandidatePositions = new List<Vector3>();

    private float movingTileChance;
    private float fallingTileChance;
    private float lifeItemSpawnChance;

    /// <summary>
    /// 스테이지 데이터 적용 (트랩, 아이템, 청크, 타일 행동, 라이프 아이템 확률 등)
    /// </summary>
    public void ApplyStageData(StageDataSO data, int currentRepeat)
    {
        int trapCount = data.trapCount + data.trapIncrementPerRepeat * currentRepeat;
        int itemCount = data.itemCount + data.itemIncrementPerRepeat * currentRepeat;

        SetTrapList(data.trapPrefabs, trapCount);
        SetItemList(data.itemPrefabs, itemCount);
        SetChunkList(data.chunkPrefabs, data.chunkCount);
        SetTileBehaviorChances(data.movingTileChance, data.fallingTileChance);
        SetLifeItem(data.lifeItemPrefab, data.lifeItemSpawnChance);
    }

    public void SetTileBehaviorChances(float movingChance, float fallingChance)
    {
        movingTileChance = movingChance;
        fallingTileChance = fallingChance;
    }

    public void SetTrapList(Trap[] list, int count)
    {
        trapPrefabList = list;
        desiredTrapCount = count;
    }

    public void SetItemList(Item[] list, int count)
    {
        itemPrefabList = list;
        desiredItemCount = count;
    }

    public void SetChunkList(GameObject[] list, int count)
    {
        chunkPrefabList = list;
        desiredchunkCount = count;
    }

    public void SetLifeItem(GameObject prefab, float chance)
    {
        lifeItemPrefab = prefab;
        lifeItemSpawnChance = chance;
    }

    /// <summary>
    /// 특정 방향으로 빈 공간(장애물 없는 거리)를 측정 (최대 maxSteps * tileUnit)
    /// </summary>
    private float GetEmptyDistanceOneDirection(Vector3 position, Vector3 direction, float tileUnit, int maxSteps)
    {
        float distance = 0f;

        for (int i = 1; i <= maxSteps; i++)
        {
            Vector3 checkPos = position + direction * tileUnit * i;
            Collider[] hits = Physics.OverlapBox(checkPos, new Vector3(tileUnit * 0.4f, tileUnit * 0.4f, tileUnit * 0.4f));

            if (hits.Length > 0)
                break;
            distance += tileUnit;
        }
        return distance;
    }

    /// <summary>
    /// 청크 맵 생성 및 이동 타일, 떨어지는 타일, 라이프 아이템 확률적 배치
    /// </summary>
    public void GenerateChunkMap()
    {
        // 이전에 있던 청크들 초기화 및 리스트 초기화
        if (chunkGroup != null)
            Destroy(chunkGroup);
        trapCandidatePositions.Clear();

        chunkGroup = new GameObject("ChunkMap");

        for (int i = 0; i < desiredchunkCount; i++)
        {
            Vector3 chunkPos = new Vector3(i * chunkDepth * tileSize, 0f, 0f);
            GameObject chunkPrefab = chunkPrefabList[UnityEngine.Random.Range(0, chunkPrefabList.Length)];
            GameObject chunkInstance = Instantiate(chunkPrefab, chunkPos, Quaternion.identity, chunkGroup.transform);

            foreach (Transform child in chunkInstance.GetComponentsInChildren<Transform>())
            {
                if (child == chunkInstance.transform) continue;

                Vector3 pos = child.position;

                // 이동 가능한 거리 체크 (X축, Z축)
                float xRange = GetEmptyDistanceOneDirection(pos, Vector3.right, tileSize, 3);
                float xRangeL = GetEmptyDistanceOneDirection(pos, Vector3.left, tileSize, 3);
                float xTotal = Mathf.Min(xRange, xRangeL);

                float zRange = GetEmptyDistanceOneDirection(pos, Vector3.forward, tileSize, 3);
                float zRangeL = GetEmptyDistanceOneDirection(pos, Vector3.back, tileSize, 3);
                float zTotal = Mathf.Min(zRange, zRangeL);

                bool canMoveX = xTotal > 0;
                bool canMoveZ = zTotal > 0;

                float rand = UnityEngine.Random.value;

                // MovingTile 붙이기 (확률과 이동 가능 여부 체크)
                if (rand < movingTileChance && (canMoveX || canMoveZ))
                {
                    MovingTile mover = child.gameObject.AddComponent<MovingTile>();

                    if (canMoveX && canMoveZ)
                    {
                        mover.moveAxis = (UnityEngine.Random.value < 0.5f) ? Vector3.right : Vector3.forward;
                        mover.moveDistance = (mover.moveAxis == Vector3.right) ? xTotal : zTotal;
                    }
                    else if (canMoveX)
                    {
                        mover.moveAxis = Vector3.right;
                        mover.moveDistance = xTotal;
                    }
                    else
                    {
                        mover.moveAxis = Vector3.forward;
                        mover.moveDistance = zTotal;
                    }
                    mover.moveSpeed = UnityEngine.Random.Range(1.5f, 3f);
                    continue; // 함정 후보에 추가하지 않음
                }
                // FallingTile 붙이기
                else if (rand < movingTileChance + fallingTileChance)
                {
                    if (child.GetComponent<FallingTile>() == null)
                        child.gameObject.AddComponent<FallingTile>();
                    continue; // 함정 후보에 추가하지 않음
                }

                // 함정 후보 위치 리스트에 추가 (y 축은 타일 높이만큼 올림)
                trapCandidatePositions.Add(new Vector3(pos.x, pos.y + tileSize, pos.z));
            }
        }

        // 맵 끝에 피니시 지점 생성
        Instantiate(finishPrefab,
            new Vector3(desiredchunkCount * chunkDepth * tileSize, 0f, 0f),
            Quaternion.identity,
            chunkGroup.transform);

        // 라이프 아이템 확률적으로 앞쪽 청크에 배치
        SpawnLifeItemIfNeeded();
    }

    /// <summary>
    /// 라이프 아이템이 출현할 확률이 되면 앞쪽 청크 위치 후보에서 랜덤 배치
    /// </summary>
    private void SpawnLifeItemIfNeeded()
    {
        if (UnityEngine.Random.value >= lifeItemSpawnChance || lifeItemPrefab == null) return;

        List<Vector3> frontSpawnCandidates = new List<Vector3>();

        foreach (Vector3 pos in trapCandidatePositions)
        {
            if (pos.x < chunkDepth * tileSize * 2f) // 앞쪽 청크 범위 설정
            {
                frontSpawnCandidates.Add(pos);
            }
        }

        if (frontSpawnCandidates.Count == 0) return;

        Vector3 randomPos = frontSpawnCandidates[UnityEngine.Random.Range(0, frontSpawnCandidates.Count)];
        GameObject lifeItem = Instantiate(lifeItemPrefab, randomPos, Quaternion.identity, chunkGroup.transform);

        Transform anchor = lifeItem.transform.Find("PlacementAnchor");
        if (anchor != null)
        {
            Vector3 offset = lifeItem.transform.position - anchor.position;
            lifeItem.transform.position += offset;
        }
    }

    /// <summary>
    /// 트랩과 아이템 위치를 무작위로 셔플해서 개수에 맞게 분리 반환
    /// </summary>
    public (List<Vector3> trapPositions, List<Vector3> itemPositions) GetRandomTrapAndItemPositions(int trapCount, int itemCount)
    {
        List<Vector3> shuffled = new List<Vector3>(trapCandidatePositions);

        // Fisher-Yates shuffle
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }

        int totalCount = Mathf.Min(trapCount + itemCount, shuffled.Count);
        int actualTrapCount = Mathf.Min(trapCount, totalCount);
        int actualItemCount = Mathf.Min(itemCount, totalCount - actualTrapCount);

        List<Vector3> trapPositions = shuffled.GetRange(0, actualTrapCount);
        List<Vector3> itemPositions = shuffled.GetRange(actualTrapCount, actualItemCount);

        return (trapPositions, itemPositions);
    }

    /// <summary>
    /// 트랩과 아이템 오브젝트를 생성
    /// </summary>
    public void CreateTrapsAndItems()
    {
        spawnedTrapNums = new int[trapPrefabList.Length];
        trapGroup = new GameObject("TrapGroup");

        (List<Vector3> trapPositions, List<Vector3> itemPositions) = GetRandomTrapAndItemPositions(desiredTrapCount, desiredItemCount);

        // 트랩 생성
        foreach (Vector3 tilePos in trapPositions)
        {
            int tNum = UnityEngine.Random.Range(0, trapPrefabList.Length);
            Trap original = trapPrefabList[tNum];
            Trap trapInstance = Instantiate(original, tilePos, original.transform.rotation);

            Transform placementAnchor = trapInstance.transform.Find("PlacementAnchor");
            Vector3 offset = trapInstance.transform.position - placementAnchor.position;
            trapInstance.transform.position = tilePos + offset;

            trapInstance.transform.SetParent(trapGroup.transform);

            totalTrapDamage += trapInstance.damage;
            spawnedTrapNums[tNum] += 1;
        }

        itemGroup = new GameObject("ItemGroup");

        // 아이템 생성
        foreach (Vector3 tilePos in itemPositions)
        {
            int iNum = UnityEngine.Random.Range(0, itemPrefabList.Length);
            Item original = itemPrefabList[iNum];
            Item itemInstance = Instantiate(original, tilePos, original.transform.rotation);

            Transform placementAnchor = itemInstance.transform.Find("PlacementAnchor");
            Vector3 offset = itemInstance.transform.position - placementAnchor.position;
            itemInstance.transform.position = tilePos + offset;

            itemInstance.transform.SetParent(itemGroup.transform);
        }
    }

    /// <summary>
    /// 맵과 관련된 모든 청크 오브젝트 제거 및 후보 위치 리스트 초기화
    /// </summary>
    public void ClearMap()
    {
        if (chunkGroup != null)
            Destroy(chunkGroup);

        trapCandidatePositions.Clear();
    }

    /// <summary>
    /// 생성된 트랩과 아이템 오브젝트 삭제 및 관련 데이터 초기화
    /// </summary>
    public void ClearTrapsAndItems()
    {
        if (trapGroup != null)
            Destroy(trapGroup);
        if (itemGroup != null)
            Destroy(itemGroup);

        if (spawnedTrapNums != null)
            Array.Clear(spawnedTrapNums, 0, spawnedTrapNums.Length);

        totalTrapDamage = 0;
    }

    /// <summary>
    /// UI에 난이도 관련 정보 갱신
    /// </summary>
    public void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(trapPrefabList, spawnedTrapNums, totalTrapDamage);
    }

    /// <summary>
    /// 스테이지를 초기화하고 청크, 트랩, 아이템을 생성 및 UI 갱신
    /// </summary>
    public void ResetStage()
    {
        ClearMap();
        GenerateChunkMap();
        ClearTrapsAndItems();
        CreateTrapsAndItems();
        ChangeStageDifficulty();
    }

    /// <summary>
    /// 트랩과 아이템만 초기화 후 재생성 및 UI 갱신
    /// </summary>
    public void ResetTrapsAndItems()
    {
        ClearTrapsAndItems();
        CreateTrapsAndItems();
        ChangeStageDifficulty();
    }
}
