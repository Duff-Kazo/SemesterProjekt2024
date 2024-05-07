using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public enum RoomType
{
    BranchRoom,
    MainPath,
    StartRoom,
    EndRoom,
    ShopRoom,
    BossRoom
}
public class RoomDefinition : IEquatable<Vector2Int>, IEquatable<RoomDefinition>
{
    public Vector2Int pos;
    public bool topConnected;
    public bool leftConnected;
    public bool rightConnected;
    public bool bottomConnected;
    public RoomType roomType;
    public List<Vector2Int> tilePositions = null;
    public List<Vector2Int> enemySpawnPoints = null;

    public RoomDefinition(Vector2Int position)
    {
        pos = position;
        topConnected = false;
        leftConnected = false;
        rightConnected = false;
        bottomConnected = false;    
        roomType = RoomType.BranchRoom;
    }

    public RoomDefinition(int x, int y) : this(new Vector2Int(x, y))
    {
    }

    public int x
    {
        get { return pos.x; }
        set { pos.x = value; }
    }
    public int y
    {
        get { return pos.y; }
        set { pos.y = value; }
    }


    public bool Equals(Vector2Int other)
    {
        return pos.Equals(other);
    }

    public bool Equals(RoomDefinition other)
    {
        return other.pos.Equals(pos);
    }
}

public class LevelDefinition
{
    public int mainPathLength;
    public int branchLength;
    public int enemyCount;
    public int headMouthWeight;
    public int crawlerWeight;
    public int eyeWeigth;
}
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap miniMapTileMap;
    [SerializeField] private TileBase mapTile;

    [SerializeField] private int turns;
    [SerializeField] private int straights;
    [SerializeField] private GameObject mainPathDot;

    [SerializeField] private int mainPathLength;
    [SerializeField] private int nOfBranchRooms;
    [Header("Corner Tiles")]
    [SerializeField] private List<GameObject> topLeftTile;
    [Header("Corridor Tiles")]
    [SerializeField] private List<GameObject> leftRightTile;
    [Header("DeadEnds")]
    [SerializeField] private List<GameObject> leftTile;
    [Header("T-Cross")]
    [SerializeField] private List<GameObject> leftTopRight;
    [Header("Cross")]
    [SerializeField] private List<GameObject> cross;

    [Header("Enemies")]
    [SerializeField] private GameObject headMouth;
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject crawler;

    private NavMeshSurface navMesh;


    private Vector3 leftRotation = new Vector3(0, 0, 90);
    private Vector3 rightRotation = new Vector3(0, 0, -90);
    private Vector3 turn = new Vector3(0, 0, 180);

    public const int TILE_SIZE = 24;

    public enum RotationType
    {
        None,
        Degree90,
        Degree180,
        Degree270
    }

    void Start()
    {
        navMesh = GetComponentInChildren<NavMeshSurface>();
        LevelDefinition level = new LevelDefinition();
        level.headMouthWeight = 10;
        level.crawlerWeight = 7;
        level.eyeWeigth = 5;
        level.mainPathLength = mainPathLength;
        level.branchLength = nOfBranchRooms;
        level.enemyCount = 75;
        List<RoomDefinition> roomSet = null;
        int numOfCries = 0;
        while (roomSet == null)
        {
            roomSet = GenerateRooms();
            numOfCries++;
        }
        Debug.Log("Ich hab " + (numOfCries - 1) + " mal BUHUHUHUI ICh Wein JeTzT, gemacht >:[");
        HashSet<Vector2Int> tilePositions = new HashSet<Vector2Int>();
        foreach (var pos in roomSet)
        {
            GenerateFloorPositions(pos);
            tilePositions.UnionWith(pos.tilePositions);
        }

        TileMapVisualizer visualizer = GetComponentInChildren<TileMapVisualizer>();
        visualizer.PaintFloorTiles(tilePositions);
        TileMapVisualizer.PaintTiles(tilePositions, miniMapTileMap, mapTile);
        WallGenerator.CreateWalls(tilePositions, visualizer);
        navMesh.BuildNavMesh();
        SpawnEnemies(roomSet, level);
    }

    private void SpawnEnemies(List<RoomDefinition> rooms, LevelDefinition level)
    {
        int enemiesToSpawn = level.enemyCount;
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        foreach (var room in rooms)
        {
            if(enemiesToSpawn <= 0)
            {
                return;
            }
            if(room.enemySpawnPoints != null && room.enemySpawnPoints.Count > 0)
            {
                int index = Random.Range(0,room.enemySpawnPoints.Count);
                Vector2 enemyPos = room.enemySpawnPoints[index];
                SpawnSingleEnemy(enemyPos, level);
                room.enemySpawnPoints.RemoveAt(index);
                enemiesToSpawn--;
                spawnPoints.AddRange(room.enemySpawnPoints);
            }
        }
        if(enemiesToSpawn > spawnPoints.Count)
        {
            throw new Exception("Not enough spawn points left");
        }
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            SpawnSingleEnemy(spawnPoints[randomIndex], level);
            spawnPoints.RemoveAt(randomIndex);
        }
    }

    private void SpawnSingleEnemy(Vector2 pos, LevelDefinition level)
    {
        int enemyType = Random.Range(0, level.eyeWeigth + level.headMouthWeight + level.crawlerWeight);
        if(enemyType < level.eyeWeigth)
        {
            Instantiate(eye, pos, Quaternion.identity);
        }
        else if(enemyType < level.eyeWeigth + level.headMouthWeight)
        {
            Instantiate(headMouth, pos, Quaternion.identity);
        }
        else
        {
            Instantiate(crawler, pos, Quaternion.identity);
        }
    }

    private List<RoomDefinition> GenerateRooms()
    {
        List<RoomDefinition> roomSet = new List<RoomDefinition>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        var startRoom = new RoomDefinition(0, 0);
        Vector2Int previousRoomPos = startRoom.pos;

        roomSet.Add(startRoom);
        roomPositions.Add(startRoom.pos);

        //Main Branch Generation
        GenerateBranch(startRoom, previousRoomPos, roomPositions, roomSet, mainPathLength - 1);
        if (roomSet.Count < mainPathLength)
        {
            return null;
        }
        foreach (var room in roomSet)
        {
            room.roomType = RoomType.MainPath;
        }
        startRoom.roomType = RoomType.StartRoom;
        roomSet.ElementAt(roomSet.Count - 1).roomType = RoomType.EndRoom;
        List<RoomDefinition> validBranchRooms = new List<RoomDefinition>();
        for (int i = 1; i < roomSet.Count - 1; i++)
        {
            validBranchRooms.Add(roomSet[i]);
        }

        //Branch Generation
        while (roomSet.Count < mainPathLength + nOfBranchRooms)
        {
            int nextIndex = Random.Range(0, validBranchRooms.Count);
            int branchLength = Random.Range(1, Mathf.RoundToInt(Mathf.Sqrt(nOfBranchRooms)) + 1);
            if (roomSet.Count + branchLength > mainPathLength + nOfBranchRooms)
            {
                branchLength = mainPathLength + nOfBranchRooms - roomSet.Count;
            }
            int result = GenerateBranch(validBranchRooms[nextIndex], validBranchRooms[nextIndex].pos, roomPositions, roomSet, branchLength);
            if (result < 1)
            {
                validBranchRooms.RemoveAt(nextIndex);
                if (validBranchRooms.Count < 1)
                {
                    return null;
                }
            }
        }
        return roomSet;
    }

    //Tatsächliches tile platzieren
    private void GenerateFloorPositions(RoomDefinition room)
    {
        GameObject prefab;
        RotationType rotation = RotationType.None;
        if (room.leftConnected && room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftRightTile);
        }
        else if (!room.leftConnected && !room.rightConnected && room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftRightTile);
            rotation = RotationType.Degree90;
        }
        else if (room.leftConnected && !room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(topLeftTile);
        }
        else if (!room.leftConnected && room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(topLeftTile);
            rotation = RotationType.Degree90;
        }
        else if (room.leftConnected && !room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(topLeftTile);
            rotation = RotationType.Degree270;
        }
        else if (!room.leftConnected && room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(topLeftTile);
            rotation = RotationType.Degree180;
        }
        else if (room.leftConnected && !room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTile);
        }
        else if (!room.leftConnected && room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTile);
            rotation = RotationType.Degree180;
        }
        else if (!room.leftConnected && !room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTile);
            rotation = RotationType.Degree90;
        }
        else if (!room.leftConnected && !room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTile);
            rotation = RotationType.Degree270;
        }
        else if (!room.leftConnected && room.rightConnected && room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTopRight);
            rotation = RotationType.Degree90;
        }
        else if (room.leftConnected && !room.rightConnected && room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTopRight);
            rotation = RotationType.Degree270;
        }
        else if (room.leftConnected && room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTopRight);
            rotation = RotationType.Degree180;
        }
        else if (room.leftConnected && room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            prefab = ChooseRandomFrom(leftTopRight);
        }
        else if (room.leftConnected && room.rightConnected && room.topConnected && room.bottomConnected)
        {
            prefab = ChooseRandomFrom(cross);
        }
        else
        {
            throw new Exception("Das jetzt kaka (er hat nix gefundndnnd)");
        }
        Tilemap[] tileMaps = prefab.GetComponentsInChildren <Tilemap>();
        Tilemap roomTileMap = null;
        Tilemap spawnPointTileMap = null;
        foreach (var tileMap in tileMaps)
        {
            if(tileMap.name == "Floor")
            {
                roomTileMap = tileMap;
            }
            else if(tileMap.name == "EnemySpawnPoints")
            {
                spawnPointTileMap = tileMap;
            }
        }
        if(roomTileMap == null)
        {
            throw new Exception("the requested tilemap named Floor was not found in prefab");
        }
        if (spawnPointTileMap == null)
        {
            throw new Exception("the requested tilemap named EnemySpawnPoints was not found in prefab");
        }
        room.tilePositions = GetTilePositionsInRoomTileMap(roomTileMap, room, rotation);
        if(room.roomType == RoomType.MainPath || room.roomType == RoomType.BranchRoom)
        {
            room.enemySpawnPoints = GetTilePositionsInRoomTileMap(spawnPointTileMap, room, rotation);
        }
    }

    private List<Vector2Int> GetTilePositionsInRoomTileMap(Tilemap tilemap, RoomDefinition room, RotationType rotation)
    {
        List<Vector2Int> roomFloorTilePositions = new List<Vector2Int>();
        BoundsInt roomBounds = tilemap.cellBounds;
        foreach (var tilePos in roomBounds.allPositionsWithin)
        {
            TileBase tileBase = tilemap.GetTile(tilePos);
            if (tileBase != null)
            {
                Vector2Int pos = new Vector2Int(tilePos.x, tilePos.y);
                switch (rotation)
                {
                    case RotationType.Degree90:
                        {
                            int temp = pos.x;
                            pos.x = pos.y;
                            pos.y = temp * -1;
                            pos.y -= 1;
                            break;
                        }
                    case RotationType.Degree180:
                        {
                            pos.x *= -1;
                            pos.y *= -1;
                            pos.x -= 1;
                            pos.y -= 1;
                            break;
                        }
                    case RotationType.Degree270:
                        {
                            int temp = pos.x;
                            pos.x = pos.y * -1;
                            pos.y = temp;
                            pos.x -= 1;
                            break;
                        }
                }
                pos.x += room.pos.x * TILE_SIZE;
                pos.y += room.pos.y * TILE_SIZE;
                roomFloorTilePositions.Add(pos);
            }
        }
        return roomFloorTilePositions;
    }



    private int GenerateBranch(RoomDefinition currentRoom, Vector2Int previousRoomPos, HashSet<Vector2Int> roomPositions, List<RoomDefinition> rooms, int numOfRoomsToGenerate)
    {
        for (int i = 0; i < numOfRoomsToGenerate; i++)
        {
            List<RoomDefinition> validNextRooms = new List<RoomDefinition>(4);
            RoomDefinition upRoom = new RoomDefinition(currentRoom.x, currentRoom.y + 1);
            upRoom.bottomConnected = true;
            RoomDefinition downRoom = new RoomDefinition(currentRoom.x, currentRoom.y - 1);
            downRoom.topConnected = true;
            RoomDefinition leftRoom = new RoomDefinition(currentRoom.x - 1, currentRoom.y);
            leftRoom.rightConnected = true;
            RoomDefinition rightRoom = new RoomDefinition(currentRoom.x + 1, currentRoom.y);
            rightRoom.leftConnected = true;
            if (!roomPositions.Contains(upRoom.pos))
            {
                if (previousRoomPos == downRoom.pos)
                {
                    for (int y = 0; y < straights; y++)
                    {
                        validNextRooms.Add(upRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turns; y++)
                    {
                        validNextRooms.Add(upRoom);
                    }
                }
            }
            if (!roomPositions.Contains(downRoom.pos))
            {
                if (previousRoomPos == upRoom.pos)
                {
                    for (int y = 0; y < straights; y++)
                    {
                        validNextRooms.Add(downRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turns; y++)
                    {
                        validNextRooms.Add(downRoom);
                    }
                }
            }
            if (!roomPositions.Contains(leftRoom.pos))
            {
                if (previousRoomPos == rightRoom.pos)
                {
                    for (int y = 0; y < straights; y++)
                    {
                        validNextRooms.Add(leftRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turns; y++)
                    {
                        validNextRooms.Add(leftRoom);
                    }
                }
            }
            if (!roomPositions.Contains(rightRoom.pos))
            {
                if (previousRoomPos == leftRoom.pos)
                {
                    for (int y = 0; y < straights; y++)
                    {
                        validNextRooms.Add(rightRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turns; y++)
                    {
                        validNextRooms.Add(rightRoom);
                    }
                }
            }

            if (validNextRooms.Count == 0)
            {
                return i;
            }

            int nextRoomIndex = Random.Range(0, validNextRooms.Count);
            var nextRoom = validNextRooms[nextRoomIndex];

            if (nextRoom.topConnected)
            {
                currentRoom.bottomConnected = true;
            }
            if (nextRoom.bottomConnected)
            {
                currentRoom.topConnected = true;
            }
            if (nextRoom.rightConnected)
            {
                currentRoom.leftConnected = true;
            }
            if (nextRoom.leftConnected)
            {
                currentRoom.rightConnected = true;
            }

            rooms.Add(nextRoom);
            roomPositions.Add(nextRoom.pos);
            previousRoomPos = currentRoom.pos;
            currentRoom = nextRoom;
        }
        return numOfRoomsToGenerate;
    }


    private GameObject ChooseRandomFrom(List<GameObject> gameObjects)
    {
        int random = Random.Range(0, gameObjects.Count);
        return gameObjects[random];
    }
}
