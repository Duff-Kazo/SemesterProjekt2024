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
        var previousRoom = new RoomType(0, 0);

        rooms.Add(previousRoom);
        roomPositions.Add(previousRoom.pos);

        for (int i = 0; i < walkLength; i++)
        {
            List<RoomType> validNextRooms = new List<RoomType>(4);
            RoomType upRoom = new RoomType(previousRoom.x, previousRoom.y + 1);
            upRoom.bottomConnected = true;
            RoomType downRoom = new RoomType(previousRoom.x, previousRoom.y - 1);
            downRoom.topConnected = true;
            RoomType leftRoom = new RoomType(previousRoom.x - 1, previousRoom.y);
            leftRoom.rightConnected = true;
            RoomType rightRoom = new RoomType(previousRoom.x + 1, previousRoom.y);
            rightRoom.leftConnected = true;
            if(!roomPositions.Contains(upRoom.pos))
            {
                validNextRooms.Add(upRoom);
            }
            if (!roomPositions.Contains(downRoom.pos))
            {
                validNextRooms.Add(downRoom);
            }
            if (!roomPositions.Contains(leftRoom.pos))
            {
                validNextRooms.Add(leftRoom);
            }
            if (!roomPositions.Contains(rightRoom.pos))
            {
                validNextRooms.Add(rightRoom);
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
                previousRoom.bottomConnected = true;
            }
            if(nextRoom.bottomConnected)
            {
                previousRoom.topConnected = true;
            }
            if(nextRoom.rightConnected)
            {
                previousRoom.leftConnected = true;
            }
            if(nextRoom.leftConnected)
            {
                previousRoom.rightConnected = true;
            }

            rooms.Add(nextRoom);
            roomPositions.Add(nextRoom.pos);
            previousRoom = nextRoom;
        }

        foreach (var pos in rooms)
        {
            Instantiate(ChooseTile(pos), new Vector3(pos.x * 12, pos.y * 12, 0), Quaternion.identity);
        }
    }

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
