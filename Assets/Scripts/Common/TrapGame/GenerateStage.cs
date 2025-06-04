using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    [SerializeField] InGameUI inGameUIDoc;
    [SerializeField] Trap[] trapPrefabList = new Trap[0];
    [SerializeField] Item[] itemPrefabList = new Item[0];

    public int[] spawnedTrapNums;
    public int totalTrapDamage;

    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject finishPrefab;

    [SerializeField] int desiredTrapCount = 10;
    [SerializeField] int desiredItemCount = 4;
    [SerializeField] int chunkCount = 5;

    private int chunkWidth = 3;
    private int chunkDepth = 4;
    private float tileSize = 2f;

    private GameObject trapGroup;
    private GameObject chunkGroup;
    private GameObject itemGroup;

    private List<Vector3> trapCandidatePositions = new List<Vector3>();

    public void GenerateChunkMap()
    {
        chunkGroup = new GameObject("ChunkMap");

        for (int i = 0; i < chunkCount; i++)
        {
            Vector3 chunkPos = new Vector3(i * chunkDepth * tileSize, 0f, 0f);

            GameObject chunkPrefab = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)];
            GameObject chunkInstance = Instantiate(chunkPrefab, chunkPos, Quaternion.identity, chunkGroup.transform);

            foreach (Transform child in chunkInstance.GetComponentsInChildren<Transform>())
            {
                if (child != chunkInstance.transform) // 자기 자신 제외
                {
                    trapCandidatePositions.Add(new Vector3(child.position.x, child.position.y + tileSize, child.position.z));
                }
            }
        }
        GameObject finishInstance = Instantiate(finishPrefab, 
            new Vector3(chunkCount * chunkDepth * tileSize, 0f, 0f), Quaternion.identity, chunkGroup.transform);
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

        foreach (Vector3 tilePos in trapPositions)
        {
            int tNum = UnityEngine.Random.Range(0, trapPrefabList.Length);
            Trap trapInstance = Instantiate(trapPrefabList[tNum], tilePos, Quaternion.identity);

            Transform placementAnchor = trapInstance.transform.Find("PlacementAnchor");
            Vector3 offset = trapInstance.transform.position - placementAnchor.position;
            trapInstance.transform.position = tilePos + offset;

            trapInstance.transform.SetParent(trapGroup.transform);

            totalTrapDamage += trapInstance.damage;
            spawnedTrapNums[tNum] += 1;
        }

        itemGroup = new GameObject("ItemGroup");

        foreach (Vector3 tilePos in itemPositions)
        {
            int iNum = UnityEngine.Random.Range(0, itemPrefabList.Length);
            Item itemInstance = Instantiate(itemPrefabList[iNum], tilePos, Quaternion.identity);

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
}
