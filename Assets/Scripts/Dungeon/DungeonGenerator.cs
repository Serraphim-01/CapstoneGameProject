using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System.Collections; // This is the namespace for NavMeshSurface

public class DungeonGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 18;
    public int gridHeight = 18;

    [Header("Room Settings")]
    public int minRoomSize = 3;
    public int maxRoomSize = 6;
    public int minRoomCount = 3; // Minimum number of rooms
    public int maxRoomCount = 10; // Maximum number of rooms

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject doorPrefab;

    [Header("Spawn Settings")]
    public GameObject spawnDoorPrefab;
    public Vector3 playerSpawnPoint;
    public GameObject playerPrefab;

    private int[,] dungeonGrid;
    private List<Rect> rooms = new List<Rect>();

    [HideInInspector] public Transform dungeonRoot;
    public NavMeshSurface navMeshSurface;


    void Start()
    {
        if (Application.isPlaying && transform.Find("Dungeon") == null)
        {
            Debug.Log("No dungeon found. Generating everything...");
            Generate();
        }
    }

    public void Generate()
    {
        ClearOldDungeon();

        dungeonGrid = new int[gridWidth, gridHeight];
        rooms.Clear();

        int roomCount = Random.Range(minRoomCount, maxRoomCount + 1); // Random room count within the range

        for (int i = 0; i < roomCount; i++)
        {
            int w = Random.Range(minRoomSize, maxRoomSize);
            int h = Random.Range(minRoomSize, maxRoomSize);
            int x = Random.Range(1, gridWidth - w - 1);
            int y = Random.Range(1, gridHeight - h - 1);

            Rect newRoom = new Rect(x, y, w, h);
            bool overlaps = false;
            Rect paddedRoom = new Rect(x - 1, y - 1, w + 2, h + 2);

            foreach (Rect room in rooms)
            {
                if (paddedRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
                CreateRoom(newRoom);

                if (rooms.Count > 1)
                {
                    Vector2Int prevCenter = GetRoomCenter(rooms[rooms.Count - 2]);
                    Vector2Int newCenter = GetRoomCenter(newRoom);
                    CreateCorridor(prevCenter, newCenter);
                }
            }
        }

        InstantiateTiles();
        PlaceWallsAndDoors();
        PlaceSpawnDoorAndPlayer();

        StartCoroutine(CompleteGenerationSequence());
    }

    void CreateRoom(Rect room)
    {
        for (int x = (int)room.xMin; x < (int)room.xMax; x++)
        {
            for (int y = (int)room.yMin; y < (int)room.yMax; y++)
            {
                dungeonGrid[x, y] = 1;
            }
        }
    }

    Vector2Int CreateCorridor(Vector2Int from, Vector2Int to)
    {
        Vector2Int doorPos = from;

        for (int x = Mathf.Min(from.x, to.x); x <= Mathf.Max(from.x, to.x); x++)
        {
            dungeonGrid[x, from.y] = 1;
            doorPos = new Vector2Int(x, from.y);
        }

        for (int y = Mathf.Min(from.y, to.y); y <= Mathf.Max(from.y, to.y); y++)
        {
            dungeonGrid[to.x, y] = 1;
            doorPos = new Vector2Int(to.x, y);
        }

        return doorPos;
    }

    Vector2Int GetRoomCenter(Rect room)
    {
        return new Vector2Int(
            Mathf.RoundToInt(room.x + room.width / 2),
            Mathf.RoundToInt(room.y + room.height / 2)
        );
    }

    void InstantiateTiles()
    {
        dungeonRoot = new GameObject("Dungeon").transform;
        dungeonRoot.SetParent(transform);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (dungeonGrid[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x * 4, 0, y * 4); // assuming 4 unit tiles
                    Instantiate(floorPrefab, pos, Quaternion.identity, dungeonRoot);
                }
            }
        }
    }

    void PlaceWallsAndDoors()
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // Up
            new Vector2Int(0, -1),  // Down
            new Vector2Int(-1, 0),  // Left
            new Vector2Int(1, 0)    // Right
        };

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (dungeonGrid[x, y] != 1) continue;

                foreach (var dir in directions)
                {
                    int nx = x + dir.x;
                    int ny = y + dir.y;

                    if (!InBounds(nx, ny) || dungeonGrid[nx, ny] == 0)
                    {
                        Vector3 wallPos = new Vector3((x + dir.x * 0.5f) * 4, 0, (y + dir.y * 0.5f) * 4);
                        Quaternion rotation = Quaternion.identity;

                        if (dir.x != 0) rotation = Quaternion.Euler(0, 90, 0);

                        Instantiate(wallPrefab, wallPos, rotation, dungeonRoot);
                    }
                }
            }
        }
    }

    bool InBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public void ClearOldDungeon()
    {
        // Clear existing baked NavMesh data
        if (navMeshSurface != null)
        {
            navMeshSurface.RemoveData();
            Debug.Log("Previous NavMesh cleared.");
        }

        // Destroy existing dungeon
        Transform oldDungeon = transform.Find("Dungeon");
        if (oldDungeon != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(oldDungeon.gameObject);
#else
        Destroy(oldDungeon.gameObject);
#endif
        }
    }

    void PlaceSpawnDoorAndPlayer()
    {
        if (rooms.Count == 0)
        {
            Debug.LogWarning("No rooms generated. Cannot place spawn door.");
            return;
        }

        Rect spawnRoom = rooms[0];
        Vector2Int doorGridPos = new Vector2Int((int)spawnRoom.xMin, Mathf.RoundToInt(spawnRoom.y + spawnRoom.height / 2));
        Vector3 doorWorldPos = new Vector3(doorGridPos.x * 4, 0, doorGridPos.y * 4);
        Quaternion doorRotation = Quaternion.Euler(0, 90, 0); // Facing into the room (along +X)

        // Make sure this is inside the room grid
        if (!InBounds(doorGridPos.x, doorGridPos.y) || dungeonGrid[doorGridPos.x, doorGridPos.y] != 1)
        {
            Debug.LogWarning("Could not place spawn door inside the spawn room.");
            return;
        }

        GameObject door = Instantiate(spawnDoorPrefab, doorWorldPos, doorRotation, dungeonRoot);
        door.name = "Spawn Door";

        // Player spawns in front of the door (into the room, toward +X)
        Vector3 offset = Vector3.right * 4;
        playerSpawnPoint = door.transform.position + offset;
    }




    public List<Rect> GetRooms()
    {
        return rooms;
    }

    private IEnumerator CompleteGenerationSequence()
{
    // Step 1: Populate Props
    PopulateProps propPopulator = GetComponent<PopulateProps>();
    if (propPopulator != null) propPopulator.Populate();

    yield return null; // Wait a frame

    // Step 2: Bake NavMesh
    if (navMeshSurface != null)
    {
            Debug.Log("Baking...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked after props.");
    }

    // Wait one more frame to make sure baking is done
    yield return new WaitForEndOfFrame();

    // Step 3: Populate Monsters (AFTER NavMesh is ready)
    PopulateMonsters monsterPopulator = GetComponent<PopulateMonsters>();
    if (monsterPopulator != null) monsterPopulator.Populate();

    // Step 4: Spawn Player
    if (playerPrefab != null)
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        player.name = "Player";
    }
    else
    {
        Debug.LogWarning("Player Prefab not assigned.");
    }
}


}

