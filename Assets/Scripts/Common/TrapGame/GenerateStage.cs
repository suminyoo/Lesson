using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    public InGameUI inGameUIDoc;

    public Trap[] trapPrefabList = new Trap[0];
    private GameObject trapGroup;
    private GameObject chunkGroup;

    public int desiredTrapCount = 15;

    public int[] spawnedTrapNums;
    public int totalTrapDamage;

    public GameObject[] chunkPrefabs;
    public GameObject finishPrefab;
    public int chunkWidth = 3;
    public int chunkDepth = 4;
    public int chunkCount = 5;
    public float tileSize = 2f;
    public List<Vector3> trapCandidatePositions = new List<Vector3>();

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
        GameObject finishInstance = Instantiate(finishPrefab, new Vector3(chunkCount * chunkDepth * tileSize, 0f, 0f), Quaternion.identity, chunkGroup.transform);
    }

    public List<Vector3> GetRandomTrapPositions(int count)
    {
        // Fisher-Yates 알고리즘으로 리스트 섞기
        for (int i = 0; i < trapCandidatePositions.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, trapCandidatePositions.Count);
            (trapCandidatePositions[i], trapCandidatePositions[randIndex]) = (trapCandidatePositions[randIndex], trapCandidatePositions[i]);
        }

        // 요청한 개수만큼 잘라서 반환
        int finalCount = Mathf.Min(count, trapCandidatePositions.Count);
        return trapCandidatePositions.GetRange(0, finalCount);
    }

    public void CreateTraps()
    {
        spawnedTrapNums = new int[trapPrefabList.Length]; // 각 종류별 개수 초기화
        trapGroup = new GameObject("TrapGroup"); // 부모 오브젝트 생성

        // 랜덤한 위치 받아오기
        List<Vector3> trapPositions = GetRandomTrapPositions(desiredTrapCount);

        foreach (Vector3 tilePos in trapPositions)
        {
            // 랜덤한 함정 선택
            int tNum = UnityEngine.Random.Range(0, trapPrefabList.Length);

            // 함정 프리팹 인스턴스 생성
            Trap trapInstance = Instantiate(trapPrefabList[tNum], tilePos, Quaternion.identity);

            // 앵커 위치 기준으로 보정
            Transform placementAnchor = trapInstance.transform.Find("PlacementAnchor");
            Vector3 offset = trapInstance.transform.position - placementAnchor.position;
            trapInstance.transform.position = tilePos + offset;

            // 그룹에 속하게 함
            trapInstance.transform.SetParent(trapGroup.transform);

            // 총 피해 및 개수 기록
            totalTrapDamage += trapInstance.damage;
            spawnedTrapNums[tNum] += 1;
        }
    }
    public void ClearMap()
    {
        if (chunkGroup != null)
            Destroy(chunkGroup);

        trapCandidatePositions.Clear();
    }

    public void ClearTraps()
    {
        if (trapGroup != null)
            Destroy(trapGroup);

        if (spawnedTrapNums != null)
            Array.Clear(spawnedTrapNums, 0, spawnedTrapNums.Length);

        totalTrapDamage = 0;
    }
    public void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(trapPrefabList, spawnedTrapNums, totalTrapDamage);
    }
}
