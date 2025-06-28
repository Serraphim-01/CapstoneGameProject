using UnityEngine;
using System.Collections.Generic;

public class PopulateProps : MonoBehaviour
{
    [Header("References")]
    public DungeonGenerator dungeonGenerator;
    public GameObject[] propPrefabs;
    public int propsPerRoom = 2;

    public void Populate()
    {
        List<Rect> rooms = dungeonGenerator.GetRooms();
        if (rooms == null) return;

        foreach (var room in rooms)
        {
            for (int i = 0; i < propsPerRoom; i++)
            {
                Vector3 pos = GetRandomPointInRoom(room);
                GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
                Instantiate(prefab, pos, Quaternion.identity, dungeonGenerator.dungeonRoot);
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
