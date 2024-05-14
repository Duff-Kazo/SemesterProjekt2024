using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator
{
    public static List<RoomDefinition> GenerateRooms(LevelDefinition level)
    {
        bool shopGenerated = false;
        List<RoomDefinition> roomSet = new List<RoomDefinition>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        var startRoom = new RoomDefinition(0, 0);
        Vector2Int previousRoomPos = startRoom.pos;

        roomSet.Add(startRoom);
        roomPositions.Add(startRoom.pos);

        //Main Branch Generation
        GenerateBranch(startRoom, previousRoomPos, roomPositions, roomSet, level.mainPathLength - 1, level.straightsWeight, level.turnWeight);
        if (roomSet.Count < level.mainPathLength)
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
        while (roomSet.Count < level.mainPathLength + level.branchLength)
        {
            int nextIndex;
            int branchLength;
            if (shopGenerated)
            {
                nextIndex = Random.Range(0, validBranchRooms.Count);
                branchLength = Random.Range(1, Mathf.RoundToInt(Mathf.Sqrt(level.branchLength)) + 1);
            }
            else
            {
                nextIndex = validBranchRooms.Count / 2;
                branchLength = Mathf.RoundToInt(Mathf.Sqrt(level.branchLength));
            }

            if (roomSet.Count + branchLength > level.mainPathLength + level.branchLength)
            {
                branchLength = level.mainPathLength + level.branchLength - roomSet.Count;
            }
            int result = GenerateBranch(validBranchRooms[nextIndex], validBranchRooms[nextIndex].pos, roomPositions, roomSet, branchLength, level.straightsWeight, level.turnWeight);
            if (result < 1)
            {
                validBranchRooms.RemoveAt(nextIndex);
                if (validBranchRooms.Count < 1)
                {
                    return null;
                }
            }
            if (!shopGenerated && result == branchLength)
            {
                shopGenerated = true;
                roomSet[roomSet.Count - 1].roomType = RoomType.ShopRoom;
            }
        }
        return roomSet;
    }

    private static int GenerateBranch(RoomDefinition currentRoom, Vector2Int previousRoomPos, HashSet<Vector2Int> roomPositions, List<RoomDefinition> rooms, int numOfRoomsToGenerate, int straightsWeight, int turnWeight)
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
                    for (int y = 0; y < straightsWeight; y++)
                    {
                        validNextRooms.Add(upRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turnWeight; y++)
                    {
                        validNextRooms.Add(upRoom);
                    }
                }
            }
            if (!roomPositions.Contains(downRoom.pos))
            {
                if (previousRoomPos == upRoom.pos)
                {
                    for (int y = 0; y < straightsWeight; y++)
                    {
                        validNextRooms.Add(downRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turnWeight; y++)
                    {
                        validNextRooms.Add(downRoom);
                    }
                }
            }
            if (!roomPositions.Contains(leftRoom.pos))
            {
                if (previousRoomPos == rightRoom.pos)
                {
                    for (int y = 0; y < straightsWeight; y++)
                    {
                        validNextRooms.Add(leftRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turnWeight; y++)
                    {
                        validNextRooms.Add(leftRoom);
                    }
                }
            }
            if (!roomPositions.Contains(rightRoom.pos))
            {
                if (previousRoomPos == leftRoom.pos)
                {
                    for (int y = 0; y < straightsWeight; y++)
                    {
                        validNextRooms.Add(rightRoom);
                    }
                }
                else
                {
                    for (int y = 0; y < turnWeight; y++)
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