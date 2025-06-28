using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;
    public GameObject playerPrefab;

    public void SpawnPlayer()
    {
        if (dungeonGenerator == null || playerPrefab == null) return;

        Vector3 spawnPos = dungeonGenerator.playerSpawnPoint;
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }
}
