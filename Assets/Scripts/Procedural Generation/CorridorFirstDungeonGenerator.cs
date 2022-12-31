using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int corriderLength = 14, corridorCount = 5;        //���� ����, ���� ����
    [SerializeField] [Range(0.0f, 1.0f)] private float roomPercent = 0.8f;      //���� ���� Ȯ��

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    //���� �켱 ����
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        //���� ���� �����, �̸� floorPosition�� ��Ƽ� ä���. �� ������ ���� potentialRoomPosition�� ��´�.
        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        //DeadEnd�� ���� ���� ���� �ǹ��Ѵ�. �ش� ���� ã�Ƴ���.
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        //�� DeadEnd�� ���� �����Ѵ�.
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        //���� �ٴڰ� �� �ٴ��� ���ļ�, ���߿� ������ ���� ���������� �ʰ� �Ѵ�.
        floorPositions.UnionWith(roomPositions);

        //�ٴڰ� ���� ĥ�Ѵ�.
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    //DeadEnds�� ���� �����Ѵ�.
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)   //DeadEnd�� ���� ���ٸ�,
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    //DeadEnds�� ã�Ƴ���.
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;            //�ֺ��� 1�̶��, DeadEnd�̴�.
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPositions.Contains(position + direction))
                    neighboursCount++;
            }
            if(neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    //������ �� ���� ���� �����ϴ� �Լ�
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    //corridorLength������ ������ corridorCount��ŭ�� ������ �����. �� ���� ���� ����� floorPositions�� ��ȯ�Ѵ�,
    //���� corridorLength ���𸶴�, ���� ��ġ�� ���Ͽ� potentialRoomPositions�� ��ȯ�Ѵ�.
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corrider = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corriderLength);
            currentPosition = corrider[corrider.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corrider);
        }
    }
}