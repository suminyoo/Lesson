using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{

    public InGameUI inGameUIDoc;

    public Trap[] trapPrefabList = new Trap[0];
    private GameObject trapGroup;
    private GameObject tileGroup;

    public GameObject normalTilePrefab;
    public GameObject slopeTilePrefab;

    //public Transform[] trapPositions = new Transform[0];

    public int desiredTrapCount = 20;
    public int[] spawnedTrapNums;
    public int totalTrapDamage;

    public int xTileCount = 41;
    public int tileSize = 2;
    private int[] zOffsets = new int[] { -2, 0, 2 };

    private HashSet<Vector2> firstFloorPositions = new HashSet<Vector2>();  // 1층 타일 위치 저장
    private HashSet<Vector2> secondFloorPositions = new HashSet<Vector2>(); // 2층 타일 위치 저장
    private HashSet<int> usedXIndices = new HashSet<int>();                 // 이미 2층이 생성된 x 인덱스 기록

    // 공통 유틸: 타일 생성 및 위치 기록
    void TrySpawnTile(Vector3 position, HashSet<Vector2> recordSet, float yLevel = 0f)
    {
        Vector2 pos2D = new Vector2(position.x, position.z);
        if (!recordSet.Contains(pos2D))
        {
            Instantiate(normalTilePrefab, new Vector3(position.x, yLevel, position.z), Quaternion.identity, transform);
            recordSet.Add(pos2D);
        }
    }

    // 공통 유틸: 2층 크기 랜덤 선택
    (Vector2Int size, int index) PickSecondFloorSize()
    {
        Vector2Int[] options = { new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(3, 2) };
        int index = UnityEngine.Random.Range(0, options.Length);
        return (options[index], index);
    }

    // 공통 유틸: 사용된 X 인덱스와 너무 가까운지 확인
    bool IsXAvailable(int index)
    {
        return !usedXIndices.Any(i => Mathf.Abs(i - index) < 3);
    }

    // 본체 함수: 맵 생성
    public void GenerateMap()
    {
        tileGroup = new GameObject("Map");         // 타일 부모 그룹 생성
        firstFloorPositions.Clear();               // 1층 좌표 초기화
        secondFloorPositions.Clear();              // 2층 좌표 초기화
        usedXIndices.Clear();                      // 사용한 x 인덱스 초기화

        // 1층 타일 생성
        for (int xIndex = 0; xIndex < xTileCount; xIndex++)
        {
            float xPos = xIndex * tileSize;

            foreach (int zOffset in zOffsets)
            {
                if (UnityEngine.Random.value > 0.7f) continue;

                Vector3 pos = new Vector3(xPos, 0f, zOffset);
                TrySpawnTile(pos, firstFloorPositions); // 중복 방지 타일 생성
            }
        }

        // 2층 생성 시도
        int regionCount = UnityEngine.Random.Range(1, 4); // 1~3개 영역
        int attempts = 0;
        int maxAttempts = 10;

        while (usedXIndices.Count < regionCount && attempts < maxAttempts)
        {
            if (SpawnSecondFloorRegion()) attempts++;
            else break;
        }
    }

    // 본체 함수: 2층 영역 생성
    bool SpawnSecondFloorRegion()
    {
        var (size, _) = PickSecondFloorSize();
        int width = size.x;
        int depth = size.y;

        int minXIndex = 3;
        int maxXIndex = xTileCount - width - 1;
        if (maxXIndex < minXIndex) return false;

        for (int i = 0; i < 10; i++)
        {
            int xStartIndex = UnityEngine.Random.Range(minXIndex, maxXIndex + 1);
            if (!IsXAvailable(xStartIndex)) continue;

            float xStart = xStartIndex * tileSize;

            int? zStart = PickZStart(depth);
            if (zStart == null) return false;

            float zStartPos = zStart.Value;
            Vector2 startPos = new Vector2(xStart, zStartPos);

            CreateSecondFloorTiles(startPos, width, depth); // 기존 함수
            CreateSlope(xStart, zStartPos, depth);          // 기존 함수

            usedXIndices.Add(xStartIndex);
            return true;
        }

        return false;
    }


    //int? null 값을 가질 수 있는 int형 변수
    int? PickZStart(int depth) //2층을 만들 수 있는 z 시작 위치를 선택
    {
        // zOffsets 리스트 복사해서 후보 리스트 생성
        List<int> zCandidates = new List<int>(zOffsets);

        // depth(깊이) 기준으로 z 범위를 넘는 위치 제거
        zCandidates.RemoveAll(z => z + (depth - 1) * tileSize > 4f);

        // 가능한 z 위치가 없다면 null 반환
        if (zCandidates.Count == 0) return null;

        // 랜덤하게 하나 선택해서 반환
        return zCandidates[UnityEngine.Random.Range(0, zCandidates.Count)];
    }
    void CreateSecondFloorTiles(Vector2 start, int width, int depth) //2층 타일 생성 필요시 1층도 함께
    {
        // width와 depth를 기준으로 2층 영역의 모든 타일을 생성
        for (int dx = 0; dx < width; dx++)
        {
            for (int dz = 0; dz < depth; dz++)
            {
                // 각 타일의 실제 x, z 좌표 계산
                float x = start.x + dx * tileSize;
                float z = start.y + dz * tileSize;
                Vector2 pos2D = new Vector2(x, z);

                // 해당 위치에 1층 타일이 없다면 생성
                if (!firstFloorPositions.Contains(pos2D))
                {
                    Instantiate(normalTilePrefab, new Vector3(x, 0f, z), Quaternion.identity, transform);
                    firstFloorPositions.Add(pos2D);
                }

                // 2층 타일 생성 및 위치 기록
                Instantiate(normalTilePrefab, new Vector3(x, 2f, z), Quaternion.identity, transform);
                secondFloorPositions.Add(pos2D);
            }
        
        }
    }
    void CreateSlope(float xStart, float zStart, int depth)
    {
        // 경사 타일이 놓일 x 위치 계산 (왼쪽 한 칸 전)
        float slopeX = xStart - tileSize;

        // 경사 타일 설치 가능한 z 위치들 저장할 리스트
        List<float> slopeZOptions = new List<float>();

        for (int dz = 0; dz < depth; dz++)
        {
            float z = zStart + dz * tileSize;

            Vector2 below = new Vector2(slopeX, z);              // 경사 타일 아래 타일
            Vector2 front = new Vector2(slopeX - tileSize, z);   // 경사 타일 앞 타일

            // 아래 타일이 없으면 생성
            if (!firstFloorPositions.Contains(below))
            {
                Instantiate(normalTilePrefab, new Vector3(below.x, 0f, below.y), Quaternion.identity, transform);
                firstFloorPositions.Add(below);
            }

            // 앞 타일이 없으면 생성
            if (!firstFloorPositions.Contains(front))
            {
                Instantiate(normalTilePrefab, new Vector3(front.x, 0f, front.y), Quaternion.identity, transform);
                firstFloorPositions.Add(front);
            }

            // 두 타일 다 있으면 경사 설치 가능한 위치로 등록
            if (firstFloorPositions.Contains(below) && firstFloorPositions.Contains(front))
            {
                slopeZOptions.Add(z);
            }
        }

        // 조건 만족하는 위치 중 무작위 선택하여 경사 타일 생성
        if (slopeZOptions.Count > 0)
        {
            float slopeZ = slopeZOptions[UnityEngine.Random.Range(0, slopeZOptions.Count)];
            Instantiate(slopeTilePrefab, new Vector3(slopeX, 2f, slopeZ), Quaternion.Euler(0f, 180f, 0f), transform);
        }
        else
        {
            Debug.LogWarning("경사 타일 생성 실패: 적절한 위치 없음");
        }
    }
    public List<Vector3> GetTrapCandidateTiles()
    {
        List<Vector3> candidates = new List<Vector3>();

        // 1층 중 2층이 위에 없는 위치만 필터링해서 후보로 추가 (Y = 2f)
        foreach (Vector2 pos in firstFloorPositions)
        {
            if (secondFloorPositions.Contains(pos)) continue;
            candidates.Add(new Vector3(pos.x, 2f, pos.y));
        }

        // 2층 타일은 모두 후보에 추가 (Y = 4f)
        foreach (Vector2 pos in secondFloorPositions)
        {
            candidates.Add(new Vector3(pos.x, 4f, pos.y));
        }

        return candidates;
    }
    public List<Vector3> GetRandomTrapPositions(int count)
    {
        // 설치 가능한 모든 후보 위치 가져오기
        List<Vector3> candidates = GetTrapCandidateTiles();

        // Fisher-Yates 알고리즘으로 리스트 섞기
        for (int i = 0; i < candidates.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, candidates.Count);
            (candidates[i], candidates[randIndex]) = (candidates[randIndex], candidates[i]);
        }

        // 요청한 개수만큼 잘라서 반환
        int finalCount = Mathf.Min(count, candidates.Count);
        return candidates.GetRange(0, finalCount);
    }

    public void CreateTraps()
    {
        totalTrapDamage = 0; // 총 피해 초기화
        spawnedTrapNums = new int[trapPrefabList.Length]; // 각 종류별 개수 초기화
        trapGroup = new GameObject("TrapGroup"); // 부모 오브젝트 생성

        // 랜덤한 위치 받아오기
        List<Vector3> trapPositions = GetRandomTrapPositions(desiredTrapCount);

        foreach (Vector3 tilePos in trapPositions)
        {
            // 랜덤한 함정 선택
            int tNum = Random.Range(0, trapPrefabList.Length);

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
    public void ClearTraps()
    {
        Destroy(trapGroup);
    }
    public void ChangeStageDifficulty()
    {
        inGameUIDoc.ChangeDifficultyUI(trapPrefabList, spawnedTrapNums, totalTrapDamage);
    }
}
