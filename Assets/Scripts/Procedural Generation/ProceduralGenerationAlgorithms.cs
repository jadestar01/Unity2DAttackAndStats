using BansheeGz.BGDatabase;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.UIElements;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    //startPosition���� Random���� walkLength��ŭ �����̸� ������� ��θ� ��ȯ�Ѵ�. HashSet�� �ߺ����� �ʴ� ���� ������ �����̴�.
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    //������ �����Ѵ�.
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(startPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }

    //BoundsInt�� Int������ ������ ��� ����ü�̴�. 
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)        //Random.value = 0.0~ 1.0f;
                {
                    if (room.size.y >= minHeight * 2)                               //�������� �ɰ� �� �ִٸ�, �ɰ���.
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)                           //�������� �ɰ� �� �ִٸ�, �ɰ���.
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)   //�������� �ɰ� �� ����, �������� �ɰ� �� ������, �ּһ������ ũ�ٸ�, �渮��Ʈ�� �־�д�.
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)                           //�������� �ɰ� �� �ִٸ�, �ɰ���.
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)                               //�������� �ɰ� �� �ִٸ�, �ɰ���.
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)   //�������� �ɰ� �� ����, �������� �ɰ� �� ������, �ּһ������ ũ�ٸ�, �渮��Ʈ�� �־�д�.
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y); //(minHeight, room.size.y - minHeight)
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

//�����¿��� offset ��ǥ���� ��� �ִ� ����ü�̴�.
public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),       //Up
        new Vector2Int(1, 0),       //Right
        new Vector2Int(0, -1),      //Down
        new Vector2Int(-1, 0)       //Left
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1, 1),       //Up-Right
        new Vector2Int(1, -1),      //Down-Right
        new Vector2Int(-1, -1),     //Down-Left
        new Vector2Int(-1, 1)       //Up-Left
    };

    public static List<Vector2Int> eightDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),       //Up
        new Vector2Int(1, 1),       //Up-Right
        new Vector2Int(1, 0),       //Right
        new Vector2Int(1, -1),      //Down-Right
        new Vector2Int(0, -1),      //Down
        new Vector2Int(-1, -1),     //Down-Left
        new Vector2Int(-1, 0),      //Left
        new Vector2Int(-1, 1)       //Up-Left
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
