using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageDataSO : ScriptableObject
{
    public string stageName;

    public int repeatCountPerStage = 3;

    [Header("Trap Settings")]
    public Trap[] trapPrefabs;
    public int trapCount;
    public int trapIncrementPerRepeat = 1;  

    [Header("Item Settings")]
    public Item[] itemPrefabs;
    public int itemCount;
    public int itemIncrementPerRepeat = 1; 

    [Header("Chunk Settings")]
    public GameObject[] chunkPrefabs;
    public int chunkCount;

    [Header("Tile Behavior Settings")]
    [Range(0f, 1f)] public float movingTileChance = 0.2f;
    [Range(0f, 1f)] public float fallingTileChance = 0.1f;

}
