using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int corriderLength = 14, corridorCount = 5;        //복도 길이, 복도 개수
    [SerializeField] [Range(0.0f, 1.0f)] private float roomPercent = 0.8f;      //방의 등장 확률

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    //복도 우선 생성
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        //복도 줄을 만들고, 이를 floorPosition에 담아서 채운다. 각 복도의 마디를 potentialRoomPosition에 담는다.
        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        //DeadEnd는 방이 없는 끝을 의미한다. 해당 방을 찾아낸다.
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        //빈 DeadEnd에 방을 생성한다.
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        //복도 바닥과 방 바닥을 합쳐서, 나중에 복도가 방을 가로지르지 않게 한다.
        floorPositions.UnionWith(roomPositions);

        //바닥과 벽을 칠한다.
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    //DeadEnds에 방을 생성한다.
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)   //DeadEnd에 방이 없다면,
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    //DeadEnds를 찾아낸다.
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;            //주변이 1이라면, DeadEnd이다.
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

    //복도의 각 마디에 방을 생성하는 함수
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

    //corridorLength길이의 복도를 corridorCount만큼의 개수로 만든다. 즉 복도 줄을 만들어 floorPositions로 반환한다,
    //또한 corridorLength 마디마다, 방의 위치로 정하여 potentialRoomPositions로 반환한다.
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