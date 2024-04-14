using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int turns;
    [SerializeField] private int straights;
    [SerializeField] private GameObject mainPathDot;

    [SerializeField] private int mainPathLength;
    [SerializeField] private int nOfBranchRooms;
    [Header("Corner Tiles")]
    [SerializeField] private GameObject topLeftTile;
    [SerializeField] private GameObject topRightTile;
    [SerializeField] private GameObject bottomLeftTile;
    [SerializeField] private GameObject bottomRightTile;
    [Header("Corridor Tiles")]
    [SerializeField] private GameObject topBottomTile;
    [SerializeField] private GameObject leftRightTile;
    [Header("DeadEnds")]
    [SerializeField] private GameObject topTile;
    [SerializeField] private GameObject bottomTile;
    [SerializeField] private GameObject leftTile;
    [SerializeField] private GameObject rightTile;
    [Header("T-Cross")]
    [SerializeField] private GameObject topRightDown;
    [SerializeField] private GameObject rightDownLeft;
    [SerializeField] private GameObject DownLeftTop;
    [SerializeField] private GameObject LeftTopRight;
    [Header("Cross")]
    [SerializeField] private GameObject cross;

    void Start()
    {

        List<RoomType> roomSet = null;
        while(roomSet == null)
        {
            roomSet = GenerateRooms();
        }


        foreach (var pos in roomSet)
        {
            GameObject newRoom = Instantiate(ChooseTile(pos), new Vector3(pos.x * 12, pos.y * 12, 0), Quaternion.identity);
            if(pos.isMainPath)
            {
                Instantiate(mainPathDot, newRoom.transform.position, Quaternion.identity);
            }
        }
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
                    Debug.Log("ICh Wein JeTzT");
                    return null;
                }
            }
        }
        return roomSet;
    }

    //Tatsächliches tile platzieren
    private GameObject ChooseTile(RoomType room)
    {
        if(room.leftConnected && room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            return leftRightTile;
        }
        if (!room.leftConnected && !room.rightConnected && room.topConnected && room.bottomConnected)
        {
            return topBottomTile;
        }
        if (room.leftConnected && !room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            return topLeftTile;
        }
        if (!room.leftConnected && room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            return topRightTile;
        }
        if (room.leftConnected && !room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            return bottomLeftTile;
        }
        if (!room.leftConnected && room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            return bottomRightTile;
        }
        if (room.leftConnected && !room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            return leftTile;
        }
        if (!room.leftConnected && room.rightConnected && !room.topConnected && !room.bottomConnected)
        {
            return rightTile;
        }
        if (!room.leftConnected && !room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            return topTile;
        }
        if (!room.leftConnected && !room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            return bottomTile;
        }
        if (!room.leftConnected && room.rightConnected && room.topConnected && room.bottomConnected)
        {
            return topRightDown;
        }
        if (room.leftConnected && !room.rightConnected && room.topConnected && room.bottomConnected)
        {
            return DownLeftTop;
        }
        if (room.leftConnected && room.rightConnected && !room.topConnected && room.bottomConnected)
        {
            return rightDownLeft;
        }
        if (room.leftConnected && room.rightConnected && room.topConnected && !room.bottomConnected)
        {
            return LeftTopRight;
        }
        if (room.leftConnected && room.rightConnected && room.topConnected && room.bottomConnected)
        {
            return cross;
        }
        throw new Exception("Das jetzt kaka");
    }

    private int GenerateBranch(RoomType currentRoom,Vector2Int previousRoomPos, HashSet<Vector2Int> roomPositions, List<RoomType> rooms, int numOfRoomsToGenerate)
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
}
