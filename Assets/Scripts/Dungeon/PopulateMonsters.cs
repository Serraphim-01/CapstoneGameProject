using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class PopulateMonsters : MonoBehaviour
{
    [Header("References")]
    public DungeonGenerator dungeonGenerator;
    public GameObject commonMonsterPrefab;
    public GameObject eliteMonsterPrefab;
    public GameObject bossMonsterPrefab;

    [Header("Spawn Settings")]
    public int totalMonsters = 10;

    public void Populate()
    {
        List<Rect> rooms = dungeonGenerator.GetRooms();
        if (rooms == null || rooms.Count < 2) return;

        Rect spawnRoom = rooms[0];
        Rect bossRoom = rooms[rooms.Count - 1];
        List<Rect> middleRooms = rooms.GetRange(1, rooms.Count - 2);

        int spawnRoomMonsters = Mathf.Max(1, totalMonsters / rooms.Count);
        SpawnMonstersInRoom(spawnRoom, commonMonsterPrefab, spawnRoomMonsters);
        SpawnMonstersInRoom(bossRoom, bossMonsterPrefab, 1);

        int remainingMonsters = totalMonsters - spawnRoomMonsters - 1;
        int monstersPerRoom = middleRooms.Count > 0 ? remainingMonsters / middleRooms.Count : 0;

        foreach (var room in middleRooms)
        {
            int half = monstersPerRoom / 2;
            SpawnMonstersInRoom(room, commonMonsterPrefab, half);
            SpawnMonstersInRoom(room, eliteMonsterPrefab, monstersPerRoom - half);
        }
    }

    void SpawnMonstersInRoom(Rect room, GameObject prefab, int count)
{
    for (int i = 0; i < count; i++)
    {
        Vector3 pos = GetRandomPointInRoom(room);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            // Valid NavMesh position found
            Instantiate(prefab, hit.position, Quaternion.identity, dungeonGenerator.dungeonRoot);
        }
        else
        {
            Debug.LogWarning($"NavMesh not found at spawn position {pos}. Monster not spawned.");
        }
    }
}


    Vector3 GetRandomPointInRoom(Rect room)
    {
        float tileSize = 4f;
        int x = Random.Range((int)room.xMin + 1, (int)room.xMax - 1);
        int y = Random.Range((int)room.yMin + 1, (int)room.yMax - 1);
        return new Vector3(x * tileSize, 0, y * tileSize);
    }
}
