using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public struct RoomType : IEquatable<Vector2Int>, IEquatable<RoomType>
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
    [SerializeField] private GameObject map;
    [SerializeField] private int walkLength;
    SimpleRandomWalkSO dings;
    void Start()
    {
        dings = new SimpleRandomWalkSO();
        dings.walkLength = 15;
        dings.startRandomlyEachIteration = false;
        HashSet<RoomType> roomPositions = new HashSet<RoomType>();

        var startRoom = new RoomType(0, 0);
        roomPositions.Add(startRoom);
        var previousRoom = startRoom;

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
            if(!roomPositions.Contains(upRoom))
            {
                validNextRooms.Add(upRoom);
            }
            if (!roomPositions.Contains(downRoom))
            {
                validNextRooms.Add(downRoom);
            }
            if (!roomPositions.Contains(leftRoom))
            {
                validNextRooms.Add(leftRoom);
            }
            if (!roomPositions.Contains(rightRoom))
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

            roomPositions.Add(nextRoom);
            previousRoom = nextRoom;
        }


        foreach (var pos in roomPositions)
        {
            Instantiate(map, new Vector3(pos.x * 12, pos.y * 12, 0), Quaternion.identity);
        }
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
