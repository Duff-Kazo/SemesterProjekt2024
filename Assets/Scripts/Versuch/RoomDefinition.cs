using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    BranchRoom,
    MainPath,
    StartRoom,
    EndRoom,
    ShopRoom,
    BossRoom
}

public enum RoomConnectionType
{
    DeadEnd,
    Corridor,
    Corner,
    TCross,
    Cross
}

public enum RotationType
{
    None,
    Degree90,
    Degree180,
    Degree270
}
public class RoomDefinition
{
    public Vector2Int pos;
    public bool topConnected;
    public bool leftConnected;
    public bool rightConnected;
    public bool bottomConnected;
    public RoomType roomType;
    public List<Vector2Int> tilePositions = null;
    public List<Vector2Int> enemySpawnPoints = null;
    public List<Vector2Int> propPositions = null;
    public List<Vector2Int> itemPositions = null;

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
    public RoomConnectionType GetRoomConnectionType()
    {
        int numOfConnections = 0;
        numOfConnections += leftConnected ? 1 : 0;
        numOfConnections += topConnected ? 1 : 0;
        numOfConnections += rightConnected ? 1 : 0;
        numOfConnections += bottomConnected ? 1 : 0;

        switch (numOfConnections)
        {
            case 1:
                return RoomConnectionType.DeadEnd;
            case 2:
                if((leftConnected && rightConnected) || (topConnected && bottomConnected))
                {
                    return RoomConnectionType.Corridor;
                }
                return RoomConnectionType.Corner;
            case 3:
                return RoomConnectionType.TCross;
            case 4:
                return RoomConnectionType.Cross;
            default:
                throw new Exception("GetRoomConnectionType: wrong number of connections detected: " + numOfConnections);
        }
    }

    public RotationType GetRotationType()
    {
        RoomConnectionType roomType = GetRoomConnectionType();
        switch(roomType)
        {
            case RoomConnectionType.DeadEnd:
                if (leftConnected)
                    return RotationType.None;
                if (topConnected)
                    return RotationType.Degree90;
                if (rightConnected)
                    return RotationType.Degree180;
                return RotationType.Degree270;
            case RoomConnectionType.Corridor:
                if (leftConnected)
                    return RotationType.None;
                return RotationType.Degree90;
            case RoomConnectionType.Corner:
                if (leftConnected && topConnected)
                    return RotationType.None;
                if (topConnected && rightConnected)
                    return RotationType.Degree90;
                if (rightConnected && bottomConnected)
                    return RotationType.Degree180;
                return RotationType.Degree270;
            case RoomConnectionType.TCross:
                if (!leftConnected)
                    return RotationType.Degree90;
                if (!topConnected)
                    return RotationType.Degree180;
                if (!rightConnected)
                    return RotationType.Degree270;
                return RotationType.None;
            case RoomConnectionType.Cross:
                return RotationType.None;
            default:
                throw new Exception("GetRotationType: Illegal state detected: " + roomType);
        }
    }
}