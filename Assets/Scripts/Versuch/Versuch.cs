using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public RoomType(Vector2Int position)
    {
        pos = position;
        topConnected = false;
        leftConnected = false;
        rightConnected = false;
        bottomConnected = false;
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
public class Versuch : MonoBehaviour
{
    [SerializeField] private int turns;
    [SerializeField] private int straights;

    [SerializeField] private int walkLength;
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
    SimpleRandomWalkSO dings;
    void Start()
    {
        dings = new SimpleRandomWalkSO();
        dings.walkLength = 15;
        dings.startRandomlyEachIteration = false;
        HashSet<RoomType> rooms = new HashSet<RoomType>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        var currentRoom = new RoomType(0, 0);
        Vector2Int previousRoomPos = currentRoom.pos;

        rooms.Add(currentRoom);
        roomPositions.Add(currentRoom.pos);

        for (int i = 0; i < walkLength; i++)
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
                if(previousRoomPos == downRoom.pos)
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
                Debug.Log("GABUD");
                return;
            }

            int nextRoomIndex = Random.Range(0, validNextRooms.Count);
            var nextRoom = validNextRooms[nextRoomIndex];

            if(nextRoom.topConnected)
            {
                currentRoom.bottomConnected = true;
            }
            if(nextRoom.bottomConnected)
            {
                currentRoom.topConnected = true;
            }
            if(nextRoom.rightConnected)
            {
                currentRoom.leftConnected = true;
            }
            if(nextRoom.leftConnected)
            {
                currentRoom.rightConnected = true;
            }

            rooms.Add(nextRoom);
            roomPositions.Add(nextRoom.pos);
            previousRoomPos = currentRoom.pos;
            currentRoom = nextRoom;
        }

        foreach (var pos in rooms)
        {
            Instantiate(ChooseTile(pos), new Vector3(pos.x * 12, pos.y * 12, 0), Quaternion.identity);
        }
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

    private HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProcedualGenerationAlgorythms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
