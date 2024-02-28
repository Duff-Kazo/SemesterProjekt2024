using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProcedualGenerationAlgorythms
{


    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walklength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int> ();
        
        path.Add (startPosition);
        var previousPosition = startPosition;

        for(int i = 0; i < walklength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add (newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add (currentPosition);
        }
        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWith, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while(roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWith)
            {
                if(Random.value < 0.5f)
                {
                    if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if(room.size.x >= minWith * 2)
                    {
                        SplitVertically(minWith, roomsQueue, room);
                    }
                    else if(room.size.x >= minWith && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWith * 2)
                    {
                        SplitVertically(minWith, roomsQueue, room);
                    }
                    else if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWith && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVertically(int minWith, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        throw new System.NotImplementedException();
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        throw new System.NotImplementedException();
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>()
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(-1,0) //LEFT
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
