using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static Cinemachine.DocumentationSortingAttribute;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<LevelDefinition> levelDefinitions = new List<LevelDefinition>();


    [SerializeField] private Tilemap miniMapTileMap;
    [SerializeField] private Tilemap floorTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private TileBase mapTile;

    [Header("Corner Tiles")]
    [SerializeField] private List<GameObject> corners;
    [Header("Corridor Tiles")]
    [SerializeField] private List<GameObject> corridors;
    [Header("DeadEnds")]
    [SerializeField] private List<GameObject> deadEnds;
    [Header("T-Cross")]
    [SerializeField] private List<GameObject> tCrossings;
    [Header("Cross")]
    [SerializeField] private List<GameObject> crossings;
    [Header("ShopRoom")]
    [SerializeField] private List<GameObject> shopRoom;
    [SerializeField] private GameObject shop;
    [Header("BossRoom")]
    [SerializeField] private GameObject bossRoom;

    [Header("Props")]
    [SerializeField] private GameObject locker;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private List<GameObject> props;

    [Header("Enemies")]
    [SerializeField] private GameObject headMouth;
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject crawler;

    [Header("Cutscenes")]
    [SerializeField] private PlayableDirector endCutScene;

    private NavMeshSurface navMesh;
    private GameObject spawnedContent;
    private GameObject exit;

    private PlayerController player;

    public const int TILE_SIZE = 24;

    private int currentLevel = 0;

    void Start()
    {
        navMesh = GetComponentInChildren<NavMeshSurface>();
        player = GetComponentInChildren<PlayerController>();
        exit = transform.Find("Exit").gameObject;
        spawnedContent = transform.Find("SpawnedContent").gameObject;
        SpawnNextLevel();
    }

    public void SpawnNextLevel()
    {
        Debug.Log(levelDefinitions.Count + " " + currentLevel);
        
        ClearLevel();
        player.transform.position = transform.position;
        if (levelDefinitions.Count > currentLevel)
        {
            GenerateLevel(levelDefinitions[currentLevel]);
            currentLevel++;
        }
        else
        {
            //Credits
            Debug.Log("RollEndCredits");
            endCutScene.Play();
        }
    }

    private void GenerateLevel(LevelDefinition level)
    {
        List<RoomDefinition> roomSet = null;
        int numOfCries = 0;
        while (roomSet == null)
        {
            roomSet = LevelGenerator.GenerateRooms(level);
            numOfCries++;
            if(numOfCries > 999)
            {
                numOfCries = 0;
                level.straightsWeight++;
            }
        }
        Debug.Log("Ich hab " + (numOfCries - 1) + " mal BUHUHUHUI ICh Wein JeTzT, gemacht >:[");
        HashSet<Vector2Int> tilePositions = new HashSet<Vector2Int>();
        foreach (var pos in roomSet)
        {
            if(pos.roomType == RoomType.EndRoom && level.hasBossRoom)
            {
                GenerateFloorPositions(pos, true);
                GameObject bossTrigger = transform.Find("BossWall").gameObject;
                Vector3 triggerPos = new Vector3(pos.pos.x * TILE_SIZE, pos.pos.y * TILE_SIZE, 0);
                switch(pos.GetRotationType())
                {
                    case RotationType.None:
                        triggerPos.x -= TILE_SIZE/2 - 0.5f;
                        bossTrigger.transform.Rotate(new Vector3(0, 0, -90));
                        break;
                    case RotationType.Degree90:
                        triggerPos.y += TILE_SIZE/2 - 0.5f;
                        bossTrigger.transform.Rotate(new Vector3(0, 0, 180));
                        break;
                    case RotationType.Degree180:
                        triggerPos.x += TILE_SIZE/2 - 0.5f;
                        bossTrigger.transform.Rotate(new Vector3(0, 0, 90));
                        break;
                    case RotationType.Degree270:
                        triggerPos.y -= TILE_SIZE/2 - 0.5f;
                        break;
                    default:
                        break;
                }
                bossTrigger.transform.position = triggerPos;
                bossTrigger.SetActive(true);
            }
            else
            {
                GenerateFloorPositions(pos, false);
            }
            
            tilePositions.UnionWith(pos.tilePositions);
        }

        TileMapVisualizer visualizer = GetComponentInChildren<TileMapVisualizer>();
        visualizer.PaintFloorTiles(tilePositions);
        TileMapVisualizer.PaintTiles(tilePositions, miniMapTileMap, mapTile);
        WallGenerator.CreateWalls(tilePositions, visualizer);
        navMesh.BuildNavMesh();
        SpawnProps(roomSet, level);
        SpawnItems(roomSet, level);
        SpawnEnemies(roomSet, level);
    }

    private void ClearLevel()
    {
        miniMapTileMap.ClearAllTiles();
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        //Delete Enemies and props
        foreach (var obj in spawnedContent.GetComponentsInChildren<Transform>())
        {
            if(obj.gameObject != spawnedContent)
            {
                Destroy(obj.gameObject);
            }
        }
    }
    
    private void SpawnEnemies(List<RoomDefinition> rooms, LevelDefinition level)
    {
        int enemiesToSpawn = level.enemyCount;
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        foreach (var room in rooms)
        {
            if (enemiesToSpawn <= 0)
            {
                return;
            }
            if (room.enemySpawnPoints != null && room.enemySpawnPoints.Count > 0)
            {
                int index = Random.Range(0, room.enemySpawnPoints.Count);
                Vector2 enemyPos = room.enemySpawnPoints[index];
                SpawnSingleEnemy(enemyPos, level);
                room.enemySpawnPoints.RemoveAt(index);
                enemiesToSpawn--;
                spawnPoints.AddRange(room.enemySpawnPoints);
            }
        }
        if (enemiesToSpawn > spawnPoints.Count)
        {
            Debug.Log("SpawnEnemies: No Spawn Points left need " + enemiesToSpawn + " more");
            return;
        }
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            SpawnSingleEnemy(spawnPoints[randomIndex], level);
            spawnPoints.RemoveAt(randomIndex);
        }
    }

    private void SpawnItems(List<RoomDefinition> rooms, LevelDefinition level)
    {
        List<RoomDefinition> validRooms = new List<RoomDefinition>();
        List<RoomDefinition> tempRooms = new List<RoomDefinition>();
        foreach (var room in rooms)
        {
            if (room.itemPositions == null || room.itemPositions.Count <= 0)
            {
                continue;
            }
            validRooms.Add(room);
        }
        if (validRooms.Count <= 0)
        {
            Debug.Log("SpawnItems: No Item positions found");
            return;
        }
        float mergeCount = validRooms.Count * 0.3f;
        int itemsLeft = level.itemCount;
        for (int i = 0; i < level.itemCount + level.propItemCount; i++)
        {
            int roomIndex = Random.Range(0, validRooms.Count);
            int positionIndex = Random.Range(0, validRooms[roomIndex].itemPositions.Count);
            RoomDefinition currentRoom = validRooms[roomIndex];
            Vector2Int pos = currentRoom.itemPositions[positionIndex];
            Vector3 propPos = new Vector3(pos.x + 1f, pos.y + 1.5f, 0);
            if(itemsLeft > 0)
            {
                SpawnSingleItem(propPos);
                itemsLeft--;
            }
            else
            {
                SpawnSingleProp(propPos);
            }
            currentRoom.itemPositions.RemoveAt(positionIndex);
            if (currentRoom.itemPositions.Count > 0)
            {
                tempRooms.Add(currentRoom);
            }
            validRooms.RemoveAt(roomIndex);
            if (validRooms.Count < mergeCount)
            {
                validRooms.AddRange(tempRooms);
                tempRooms.Clear();
            }
            if (validRooms.Count <= 0)
            {
                Debug.Log("SpawnItems: NONO Item positions left, could spawn " + i + " Items");
                return;
            }
        }
    }

    private void SpawnSingleItem(Vector3 pos)
    {
        List<GameObject> validItems = new List<GameObject>();
        foreach (var item in items)
        {
            switch (item.name)
            {
                case "BetterEyes":
                    if(!PlayerController.eyesActivated)
                    {
                        validItems.Add(item);
                    }
                    break;
                case "Orbs":
                    if (!PlayerController.orbsActivated)
                    {
                        validItems.Add(item);
                    }
                    break;
                case "Plague":
                    if (!PlayerController.plagueActivated)
                    {
                        validItems.Add(item);
                    }
                    break;
                case "Shield":
                    if (!PlayerController.shieldActivated)
                    {
                        validItems.Add(item);
                    }
                    break;
                default:
                    throw new Exception("SpawnSingleItem: unknown item encountered: " + item.name);
            }
        }
        SpawnObject(validItems[Random.Range(0,validItems.Count)], pos);
    }

    private void SpawnSingleProp(Vector3 pos)
    {
        SpawnObject(props[Random.Range(0, props.Count)], pos);
    }

    private void SpawnProps(List<RoomDefinition> rooms, LevelDefinition level)
    {
        List<RoomDefinition> validRooms = new List<RoomDefinition>();
        List<RoomDefinition> tempRooms = new List<RoomDefinition>();
        foreach (var room in rooms)
        {
            if(room.propPositions == null || room.propPositions.Count <= 0)
            {
                continue;
            }
            if(room.roomType == RoomType.ShopRoom)
            {
                Vector2Int pos = room.propPositions[0];
                Vector3 shopPos = new Vector3(pos.x, pos.y + 0.5f, 0);
                shop.transform.position = shopPos;
            }
            else if(room.roomType == RoomType.EndRoom)
            {
                int index = Random.Range(0, room.propPositions.Count);
                Vector2Int pos = room.propPositions[index];
                Vector3 exitPos = new Vector3(pos.x + 0.5f, pos.y + 1.5f, 0);
                exit.transform.position = exitPos;
            }
            else
            {
                validRooms.Add(room);
            }
        }
        if(validRooms.Count <= 0)
        {
            Debug.Log("WARNING: No Locker positions found");
            return;
        }
        float mergeCount = validRooms.Count * 0.3f;
        for (int i = 0; i < level.lockerCount; i++)
        {
            int roomIndex = Random.Range(0, validRooms.Count);
            int positionIndex = Random.Range(0, validRooms[roomIndex].propPositions.Count);
            RoomDefinition currentRoom = validRooms[roomIndex];
            Vector2Int pos = currentRoom.propPositions[positionIndex];
            Vector3 propPos = new Vector3(pos.x + 0.5f, pos.y + 1.5f, 0);
            SpawnObject(locker, propPos);
            currentRoom.propPositions.RemoveAt(positionIndex);
            if(currentRoom.propPositions.Count > 0)
            {
                tempRooms.Add(currentRoom);
            }
            validRooms.RemoveAt(roomIndex);
            if(validRooms.Count < mergeCount)
            {
                validRooms.AddRange(tempRooms);
                tempRooms.Clear();
            }
            if(validRooms.Count <= 0)
            {
                Debug.Log("WARNING: NONO spind positions left, could spawn " + i + " lockers");
                return;
            }
        }
    }

    private void SpawnObject(GameObject prefab, Vector3 pos)
    {
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);
        spawned.transform.parent = spawnedContent.transform;
    }

    private void SpawnSingleEnemy(Vector2 pos, LevelDefinition level)
    {
        int enemyType = Random.Range(0, level.eyeWeigth + level.headMouthWeight + level.crawlerWeight);
        pos.x += 0.5f;
        pos.y += 1.5f;
        if(enemyType < level.eyeWeigth)
        {
            SpawnObject(eye, pos);
        }
        else if(enemyType < level.eyeWeigth + level.headMouthWeight)
        {
            SpawnObject(headMouth, pos);
        }
        else
        {
            SpawnObject(crawler, pos);
        }
    }

    

    //Tatsächliches tile platzieren
    private void GenerateFloorPositions(RoomDefinition room, bool isBossRoom)
    {
        GameObject prefab;
        RotationType rotation = room.GetRotationType();
        RoomConnectionType roomConnectionType = room.GetRoomConnectionType();
        if(room.roomType == RoomType.ShopRoom)
        {
            prefab = ChooseRandomFrom(shopRoom);
        }
        else if(isBossRoom)
        {
            prefab = bossRoom;
        }
        else
        {
            switch (roomConnectionType)
            {
                case RoomConnectionType.DeadEnd:
                    prefab = ChooseRandomFrom(deadEnds);
                    break;
                case RoomConnectionType.Corridor:
                    prefab = ChooseRandomFrom(corridors);
                    break;
                case RoomConnectionType.Corner:
                    prefab = ChooseRandomFrom(corners);
                    break;
                case RoomConnectionType.TCross:
                    prefab = ChooseRandomFrom(tCrossings);
                    break;
                case RoomConnectionType.Cross:
                    prefab = ChooseRandomFrom(crossings);
                    break;
                default:
                    throw new Exception("GenerateFloorPositions: illegal state: " + roomConnectionType);
            }
        }
        Tilemap[] tileMaps = prefab.GetComponentsInChildren <Tilemap>();
        Tilemap roomTileMap = null;
        Tilemap spawnPointTileMap = null;
        Tilemap propPositionTileMap = null;
        Tilemap itemPositionTileMap = null;
        string propTileMapName;
        switch (rotation)
        {
            case RotationType.Degree270:
                propTileMapName = "Props270";
                break;
            case RotationType.Degree180:
                propTileMapName = "Props180";
                break;
            case RotationType.Degree90:
                propTileMapName = "Props90";
                break;
            case RotationType.None:
            default:
                propTileMapName = "Props";
                break;
        }
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
            else if(tileMap.name == propTileMapName)
            {
                propPositionTileMap = tileMap;
            }
            else if(tileMap.name == "Items")
            {
                itemPositionTileMap = tileMap;
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
        if(propPositionTileMap != null)
        {
            room.propPositions = GetTilePositionsInRoomTileMap(propPositionTileMap, room, rotation);
        }
        if(room.roomType == RoomType.MainPath || room.roomType == RoomType.BranchRoom)
        {
            room.enemySpawnPoints = GetTilePositionsInRoomTileMap(spawnPointTileMap, room, rotation);
        }
        if(itemPositionTileMap != null)
        {
            room.itemPositions = GetTilePositionsInRoomTileMap(itemPositionTileMap, room, rotation);
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
    private GameObject ChooseRandomFrom(List<GameObject> gameObjects)
    {
        int random = Random.Range(0, gameObjects.Count);
        return gameObjects[random];
    }
}
