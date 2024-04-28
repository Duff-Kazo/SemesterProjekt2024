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

public class RoomType : IEquatable<Vector2Int>, IEquatable<RoomType>
{
    public Vector2Int pos;
    public bool topConnected;
    public bool leftConnected;
    public bool rightConnected;
    public bool bottomConnected;
    public bool isMainPath;
    public bool isStartRoom;
    public bool isEndRoom;

    public RoomType(Vector2Int position)
    {
        pos = position;
        topConnected = false;
        leftConnected = false;
        rightConnected = false;
        bottomConnected = false;
        isMainPath = false;
        isStartRoom = false;
        isEndRoom = false;
    }

    public RoomType(int x, int y) : this(new Vector2Int(x, y))
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

    public bool Equals(RoomType other)
    {
        return other.pos.Equals(pos);
    }
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
        List<RoomType> roomSet = null;
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
            tilePositions.UnionWith(GenerateFloorPositions(pos));
            //if(pos.isMainPath)
            //{
            //    Instantiate(mainPathDot, newRoom.transform.position, Quaternion.identity);
            //}
        }

        TileMapVisualizer visualizer = GetComponentInChildren<TileMapVisualizer>();
        visualizer.PaintFloorTiles(tilePositions);
        TileMapVisualizer.PaintTiles(tilePositions, miniMapTileMap, mapTile);
        WallGenerator.CreateWalls(tilePositions, visualizer);
    }

    private List<RoomType> GenerateRooms()
    {
        List<RoomType> roomSet = new List<RoomType>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        var startRoom = new RoomType(0, 0);
        startRoom.isStartRoom = true;
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
            room.isMainPath = true;
        }
        roomSet.ElementAt(roomSet.Count - 1).isEndRoom = true;
        List<RoomType> validBranchRooms = new List<RoomType>();
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
    private List<Vector2Int> GenerateFloorPositions(RoomType room)
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
        List<Vector2Int> roomFloorTilePositions = new List<Vector2Int>();
        Tilemap roomTilemap = prefab.GetComponentInChildren<Tilemap>();
        BoundsInt roomBounds = roomTilemap.cellBounds;
        foreach (var tilePos in roomBounds.allPositionsWithin)
        {
            TileBase tileBase = roomTilemap.GetTile(tilePos);
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
        Debug.Log("roomFloorList " + roomFloorTilePositions.Count);
        //Destroy(newRoom);
        return roomFloorTilePositions;
    }

    private int GenerateBranch(RoomType currentRoom, Vector2Int previousRoomPos, HashSet<Vector2Int> roomPositions, List<RoomType> rooms, int numOfRoomsToGenerate)
    {
        for (int i = 0; i < numOfRoomsToGenerate; i++)
        {
            List<RoomType> validNextRooms = new List<RoomType>(4);
            RoomType upRoom = new RoomType(currentRoom.x, currentRoom.y + 1);
            upRoom.bottomConnected = true;
            RoomType downRoom = new RoomType(currentRoom.x, currentRoom.y - 1);
            downRoom.topConnected = true;
            RoomType leftRoom = new RoomType(currentRoom.x - 1, currentRoom.y);
            leftRoom.rightConnected = true;
            RoomType rightRoom = new RoomType(currentRoom.x + 1, currentRoom.y);
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
