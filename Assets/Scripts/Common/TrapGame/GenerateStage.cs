using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] GameObject finishPrefab;

    private Trap[] trapPrefabList = new Trap[0];
    private Item[] itemPrefabList = new Item[0];
    private GameObject[] chunkPrefabList = new GameObject[0];

    private int desiredTrapCount;
    private int desiredItemCount;
    private int desiredchunkCount;

    public int[] spawnedTrapNums;
    public int totalTrapDamage;

    private int chunkDepth = 4;
    private float tileSize = 2f;

    private GameObject trapGroup;
    private GameObject chunkGroup;
    private GameObject itemGroup;

    private List<Vector3> trapCandidatePositions = new List<Vector3>();

    private float movingTileChance;
    private float fallingTileChance;

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

    private float GetEmptyDistanceOneDirection(Vector3 position, Vector3 direction, float tileUnit, int maxSteps)
    {
        float distance = 0f;

        for (int i = 1; i <= maxSteps; i++)
        {
            Vector3 checkPos = position + direction * tileUnit * i;
            Collider[] hits = Physics.OverlapBox(checkPos,
                new Vector3(tileUnit * 0.4f, tileUnit * 0.4f, tileUnit * 0.4f));

            if (hits.Length > 0)
                break;
            distance += tileUnit;
        }
        return distance;
    }
    public void GenerateChunkMap()
    {
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

                // X축 이동 가능 거리
                float xRange = GetEmptyDistanceOneDirection(pos, Vector3.right, tileSize, 3);
                float xRangeL = GetEmptyDistanceOneDirection(pos, Vector3.left, tileSize, 3);
                float xTotal = Mathf.Min(xRange, xRangeL);

                // Z축 이동 가능 거리
                float zRange = GetEmptyDistanceOneDirection(pos, Vector3.forward, tileSize, 3);
                float zRangeL = GetEmptyDistanceOneDirection(pos, Vector3.back, tileSize, 3);
                float zTotal = Mathf.Min(zRange, zRangeL);

                bool canMoveX = xTotal > 0;
                bool canMoveZ = zTotal > 0;

                // 랜덤값 준비
                float rand = UnityEngine.Random.value;

                // 확률로 MovingTile 붙이기
                if (rand < movingTileChance && (canMoveX || canMoveZ))
                {
                    MovingTile mover = child.gameObject.AddComponent<MovingTile>();

                    if (canMoveX && canMoveZ)
                    {
                        if (UnityEngine.Random.value < 0.5f)
                        {
                            mover.moveAxis = Vector3.right;
                            mover.moveDistance = xTotal;
                        }
                        else
                        {
                            mover.moveAxis = Vector3.forward;
                            mover.moveDistance = zTotal;
                        }
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
                    continue; // 함정 후보 제외
                }
                // 확률로 FallingTile 붙이기
                else if (rand < movingTileChance + fallingTileChance)
                {
                    if (child.GetComponent<FallingTile>() == null)
                        child.gameObject.AddComponent<FallingTile>();
                    continue; // 함정 후보 제외
                }

                // 나머지는 함정 후보에 추가
                trapCandidatePositions.Add(new Vector3(pos.x, pos.y + tileSize, pos.z));
            }
        }

        GameObject finishInstance = Instantiate(
            finishPrefab,
            new Vector3(desiredchunkCount * chunkDepth * tileSize, 0f, 0f),
            Quaternion.identity,
            chunkGroup.transform);
    }

    public (List<Vector3> trapPositions, List<Vector3> itemPositions) GetRandomTrapAndItemPositions(int trapCount, int itemCount)
    {
        List<Vector3> shuffled = new List<Vector3>(trapCandidatePositions);

        // Fisher-Yates 알고리즘으로 섞기
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

    public void CreateTrapsAndItems()
    {
        spawnedTrapNums = new int[trapPrefabList.Length];
        trapGroup = new GameObject("TrapGroup"); 

        (List<Vector3> trapPositions, List<Vector3> itemPositions) = GetRandomTrapAndItemPositions(desiredTrapCount, desiredItemCount);

        //traps
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

        //items
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
    public void ClearMap()
    {
        if (chunkGroup != null)
            Destroy(chunkGroup);

        trapCandidatePositions.Clear();
    }

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
    public void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(trapPrefabList, spawnedTrapNums, totalTrapDamage);
    }

    public void ResetStage()
    {
        ClearMap();
        GenerateChunkMap();
        ClearTrapsAndItems();
        CreateTrapsAndItems();
        ChangeStageDifficulty();
    }

    public void ResetTrapsAndItems()
    {
        ClearTrapsAndItems();
        CreateTrapsAndItems();
        ChangeStageDifficulty();
    }
}
